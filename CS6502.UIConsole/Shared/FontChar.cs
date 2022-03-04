using System;
using System.Text;

namespace CS6502.UIConsole.Shared
{
    internal class FontChar
    {
        public FontChar(
            int width,
            int height,
            byte[] data)
        {
            if (data.Length != height)
            {
                throw new ArgumentException("Font character pixel data of incorrect size");
            }

            Width = width;
            Height = height;
            Data = data;
        }

        public int Width { get; }

        public int Height { get; }

        public byte[] Data { get; }

        public byte GetPixelValue(int row, int col)
        {
            if (col >= Width)
            {
                throw new ArgumentException("Column index greater than pixel width");
            }

            if (row >= Height)
            {
                throw new ArgumentException("Row index greater than pixel height");
            }

            byte rowValue = Data[row];

            return
                (byte)(rowValue & (1 << col)) > 0
                    ? (byte)1
                    : (byte)0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    byte pixel = GetPixelValue(i, j);
                    sb.Append((pixel > 0) ? 'O' : ' ');
                }
                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
