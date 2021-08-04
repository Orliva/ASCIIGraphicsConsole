using System.Drawing;

namespace ASCIIGraphics
{
    public class BitmapToAsciiConverter
    {
        //Наборы символов для имитации различной яркости пикселей
        private readonly char[] _asciiTable = { '.', ',', ':', '+', '*', '?', '%', '$', '#', '@' };
        private readonly char[] _asciiTableNegative = { '@', '#', '$', '%', '?', '*', '+', ':', ',', '.' };

        private readonly Bitmap _bitmap;
    
        public BitmapToAsciiConverter(Bitmap bm)
        {
            _bitmap = bm;
        }

        public char[][] Convert()
        {
            return Convert(_asciiTable);
        }
        public char[][] ConvertNegative()
        {
            return Convert(_asciiTableNegative);
        }

        private char[][] Convert(char[] tableAscii)
        {
            var result = new char[_bitmap.Height][];

            for (int y = 0; y < _bitmap.Height; y++)
            {
                result[y] = new char[_bitmap.Width];
                for (int x = 0; x < _bitmap.Width; x++)
                {
                    int mapIndex = (int)Map(_bitmap.GetPixel(x, y).R, 0, 255, 0, 9); //Преобразуем диапозон значений (0;255) в (0;9)
                    result[y][x] = tableAscii[mapIndex]; //итоговый массив ASCII символов для вывода в консоль
                }
            }
            return result;
        }

        private float Map(float valueToMap, float start1, float stop1, float start2, float stop2)
        {
            return ((valueToMap - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
    }
}
