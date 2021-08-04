using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace ASCIIGraphics
{
    class Program
    {
        //Коэф-ты для корретного отображения картинки
        private const double WIDTH_OFFSET = 2;
        private const int MAX_WIDTH = 300;


        /// <summary>
        /// При некорректном отображении картинки, изменить настройки консоли
        /// Размер шрифта, тип шрифта, размер консоли
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        static void Main(string[] args)
        {
           // Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
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
                bm = ResizeBitmap(bm); //Получаем новый Bitmap измененного размера (с учетом коэф-ов)
                bm.ToGray(); //Красим пиксели Bitmap'a в градиент серого

                var converter = new BitmapToAsciiConverter(bm);
                var rows = converter.Convert(); //Получаем массив символов ASCII для вывода в консоль
                foreach (var row in rows)
                    Console.WriteLine(row);

                var rowsNegative = converter.ConvertNegative(); //Получаем картинку в "негативе" для вывода на белом (дефолтном) цвете .txt файла
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
