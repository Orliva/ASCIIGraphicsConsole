using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ASCIIGraphics
{
    class Program
    {
        private const double WIDTH_OFFSET = 2;
        private const int MAX_WIDTH = 300;

        [STAThread]
        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            var openFileDialog = new OpenFileDialog()
            {
           //     Filter = "*.bmp; | *.png; | *.jpg; | *.JPEG;"
            };

            Console.WriteLine("Press \"Enter\" to start...\n");
            while (true)
            {

                Console.ReadLine();

                if (openFileDialog.ShowDialog() != DialogResult.OK)
                    continue;
                Console.Clear();

                Bitmap bm = new Bitmap(openFileDialog.FileName);
                bm = ResizeBitmap(bm);
                bm.ToGray();

                var converter = new BitmapToAsciiConverter(bm);
                var rows = converter.Convert();
                foreach (var row in rows)
                    Console.WriteLine(row);

                var rowsNegative = converter.ConvertNegative();
                File.WriteAllLines("Image.txt", rowsNegative.Select(r=>new string(r)));

                Console.SetCursorPosition(0, 0);
            }

        }
        private static Bitmap ResizeBitmap(Bitmap bitmap)
        {
            var maxHeight = bitmap.Height / WIDTH_OFFSET * MAX_WIDTH / bitmap.Width;
            if (bitmap.Width > MAX_WIDTH || bitmap.Height > maxHeight)
                bitmap = new Bitmap(bitmap, new Size(MAX_WIDTH, (int)maxHeight));
            return bitmap;
        }
    }
}
