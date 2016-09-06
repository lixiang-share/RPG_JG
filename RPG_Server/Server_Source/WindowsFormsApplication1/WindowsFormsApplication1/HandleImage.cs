using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class HandleImage
    {
        static string loadPath = @"E:\NewProject\Test\rst\";
        public void HandleImg()
        {
            string[] files = Directory.GetFiles(loadPath);
            for (int i = 0; i < files.Length; i++)
            {
                Bitmap img = new Bitmap(files[i]);
                Bitmap bTemp = RevPicUD(img);
                Save(bTemp, files[i]);
            }
        }


         public Bitmap RevPicUD(Bitmap mybm)
         {

             int height = mybm.Size.Height;
             int width = mybm.Size.Width;
            Bitmap bm = new Bitmap(width, height);
             int x, y, z;
            Color pixel;
            for (x = 0; x < width; x++)
             {
                 for (y = height - 1, z = 0; y >= 0; y--)
                 {
                     pixel = mybm.GetPixel(x, y);//获取当前像素的值
                     bm.SetPixel(x, z++, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                 }
            }
            return bm;
         }

        public Bitmap RevPicLR(Bitmap mybm)
         {
             int height = mybm.Size.Height;
             int width = mybm.Size.Width;
             Bitmap bm = new Bitmap(width, height);
            int x, y, z; //x,y是循环次数,z是用来记录像素点的x坐标的变化的
             Color pixel;
             for (y = height - 1; y >= 0; y--)
            {
                 for (x = width - 1, z = 0; x >= 0; x--)
                {
                     pixel = mybm.GetPixel(x, y);//获取当前像素的值
                     bm.SetPixel(z++, y, Color.FromArgb(pixel.R, pixel.G, pixel.B));//绘图
                 }
           }
             return bm;
         }


        public void Save(Bitmap bitMap , string path)
        {
            path = path.Replace("rst", "handled");
            bitMap.Save(path, ImageFormat.Jpeg);
        }
    }
}
