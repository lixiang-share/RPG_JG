using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgPackDem0
{
    public class PhotoEntry
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public byte[] Image { get; set; }
        private readonly List<string> _tags = new List<string>();
        // Note that non-null read-only collection members are OK (of course, collections themselves must not be readonly.)
        public IList<string> Tags { get { return this._tags; } }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var targetObject =
                new PhotoEntry
                {
                    Id = 123,
                    Title = "My photo",
                    Date = DateTime.Now,
                    Image = new byte[] { 1, 2, 3, 4 },
                    Comment = "This is test object to be serialize/deserialize using MsgPack."
                };
            targetObject.Tags.Add("Sample");
            targetObject.Tags.Add("Excellent");
            var stream = new MemoryStream();

            // 1. Create serializer instance.
            var serializer = MessagePackSerializer.Get<PhotoEntry>();

            // 2. Serialize object to the specified stream.
            serializer.Pack(stream, targetObject);

            // Set position to head of the stream to demonstrate deserialization.
            stream.Position = 0;

            // 3. Deserialize object from the specified stream.
            var deserializedObject = serializer.Unpack(stream);

            // Test deserialized value.
            Console.WriteLine("Same object? {0}", Object.ReferenceEquals(targetObject, deserializedObject));
            Console.WriteLine("Same Id? {0}", targetObject.Id == deserializedObject.Id);
            Console.WriteLine("Same Title? {0}", targetObject.Title == deserializedObject.Title);
            // Note that MsgPack defacto-standard is Unix epoc in milliseconds precision, so micro- and nano- seconds will be lost. See sample 04 for workaround.
            Console.WriteLine("Same Date? {0}", targetObject.Date.ToString("YYYY-MM-DD HH:mm:ss.fff") == deserializedObject.Date.ToString("YYYY-MM-DD HH:mm:ss.fff"));
            // Image and Comment tests are ommitted here.
            // Collection elements are deserialzed.
            Console.WriteLine("Items count: {0}", deserializedObject.Tags.Count);
            Console.ReadKey();
        }

    }
}
