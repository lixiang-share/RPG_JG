using MsgPack;
using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgPackDemo_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var targetObject = new PackableUnpackableObject();

            var stream = new MemoryStream();

            // You can serialize/deserialize objects which implement IPackable and/or IUnpackable as usual.

            var serializer = MessagePackSerializer.Get<PackableUnpackableObject>();
            var ser = MessagePackSerializer.UnpackMessagePackObject(stream);
            serializer.Pack(stream, targetObject);
            stream.Position = 0;
            var deserializedObject = serializer.Unpack(stream);
        }
    }

    public class PackableUnpackableObject : IPackable, IUnpackable
    {
        // Imagine whien you cannot use auto-generated serializer so you have to implement custom serializer for own type easily.
        public long Id { get; set; }
        public string Name { get; set; }

        public void PackToMessage(Packer packer, PackingOptions options)
        {
            // Pack fields are here:
            // First, record total fields size.
            packer.PackArrayHeader(2);
            packer.Pack(this.Id);
            packer.PackString(this.Name);

            // ...Instead, you can pack as map as follows:
            // packer.PackMapHeader( 2 );
            // packer.Pack( "Id" );
            // packer.Pack( this.Id );
            // packer.Pack( "Name" );
            // packer.Pack( this.Name );
        }

        public void UnpackFromMessage(Unpacker unpacker)
        {
            // Unpack fields are here:

            // temp variables
            long id;
            string name;

            // It should be packed as array because we use hand-made packing implementation above.
            if (!unpacker.IsArrayHeader)
            {
                throw SerializationExceptions.NewIsNotArrayHeader();
            }

            // Check items count.
            if (UnpackHelpers.GetItemsCount(unpacker) != 2)
            {
                throw SerializationExceptions.NewUnexpectedArrayLength(2, UnpackHelpers.GetItemsCount(unpacker));
            }

            // Unpack fields here:
            if (!unpacker.ReadInt64(out id))
            {
                throw SerializationExceptions.NewMissingProperty("Id");
            }

            this.Id = id;

            if (!unpacker.ReadString(out name))
            {
                throw SerializationExceptions.NewMissingProperty("Name");
            }

            this.Name = name;

            // ...Instead, you can unpack from map as follows:
            //if ( !unpacker.IsMapHeader )
            //{
            //	throw SerializationExceptions.NewIsNotMapHeader();
            //}

            //// Check items count.
            //if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
            //{
            //	throw SerializationExceptions.NewUnexpectedArrayLength( 2, UnpackHelpers.GetItemsCount( unpacker ) );
            //}

            //// Unpack fields here:
            //for ( int i = 0; i < 2 /* known count of fields */; i++ )
            //{
            //	// Unpack and verify key of entry in map.
            //	string key;
            //	if ( !unpacker.ReadString( out key ) )
            //	{
            //		// Missing key, incorrect.
            //		throw SerializationExceptions.NewUnexpectedEndOfStream();
            //	}

            //	switch ( key )
            //	{
            //		case "Id":
            //		{
            //			if ( !unpacker.ReadInt64( out id ) )
            //			{
            //				throw SerializationExceptions.NewMissingProperty( "Id" );
            //			}
            //
            //          this.Id = id;
            //			break;
            //		}
            //		case "Name":
            //		{
            //			if ( !unpacker.ReadString( out name ) )
            //			{
            //				throw SerializationExceptions.NewMissingProperty( "Name" );
            //			}
            //
            //          this.Name = name;
            //			break;
            //		}

            //		// Note: You should ignore unknown fields for forward compatibility.
            //	}
            //}
        }
    }
}
