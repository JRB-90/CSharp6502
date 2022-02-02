using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CS6502.UIConsole.Shared;
using System;

namespace CS6502.UIConsole.Models
{
    internal class PixelCanvasModel
    {
        readonly BitmapFont font;

        public PixelCanvasModel(
            int width,
            int height,
            int charWidth,
            int charHeight,
            BitmapFont font)
        {
            Width = width;
            Height = height;
            CharWidth = charWidth;
            CharHeight = charHeight;
            CharsPerRow = width / charWidth;
            CharsPerCol = height / charHeight;

            this.font = font;

            Bitmap =
                new WriteableBitmap(
                    new PixelSize(width, height),
                    new Vector(96, 96),
                    PixelFormat.Bgra8888,
                    AlphaFormat.Opaque
                );

            Background = Color.FromRgb(0, 0, 0);
            Foreground = Color.FromRgb(0, 255, 0);

            Reset();
        }

        public int Width { get; }

        public int Height { get; }

        public int CharWidth { get; }

        public int CharHeight { get; }

        public int CharsPerRow { get; }

        public int CharsPerCol { get; }

        public Color Background { get; set; }

        public Color Foreground { get; set; }

        public WriteableBitmap Bitmap { get; }

        public void DrawCharData(byte[] charData)
        {
            if (charData.Length < CharsPerRow * CharsPerCol)
            {
                throw new ArgumentException("Incorrect number of chars passed");
            }

            Reset();
            DrawChars(charData);
        }

        private unsafe void Reset()
        {
            using (var buf = Bitmap.Lock())
            {
                var ptr = (uint*)buf.Address;
                var w = Bitmap.PixelSize.Width;
                var h = Bitmap.PixelSize.Height;

                for (var i = 0; i < w * h; i++)
                {
                    *(ptr + i) = 
                        (uint)(0xFF000000 | Background.R << 16 | Background.G << 8 | Background.B);
                }
            }
        }

        private unsafe void DrawChars(byte[] charData)
        {
            using (var buf = Bitmap.Lock())
            {
                var ptr = (uint*)buf.Address;
                var w = Bitmap.PixelSize.Width;
                var h = Bitmap.PixelSize.Height;

                for (int cy = 0; cy < CharsPerCol; cy++)
                {
                    for (int cx = 0; cx < CharsPerRow; cx++)
                    {
                        int pixX = cx * 8;
                        int pixY = cy * 8;
                        //byte charIndex = (byte)(((cy * charsPerRow + cx) % (127 - 32)) + 32);
                        byte charIndex = charData[cy * CharsPerRow + cx];
                        FontChar fontChar = font.AsciiToFontChar(charIndex);

                        for (int py = 0; py < 8; py++)
                        {
                            for (int px = 0; px < 8; px++)
                            {
                                int index = (pixY + py) * Bitmap.PixelSize.Width + (pixX + px);
                                if (fontChar.GetPixelValue(py, px) > 0)
                                {
                                    *(ptr + index) =
                                        (uint)(0xFF000000 | Foreground.R << 16 | Foreground.G << 8 | Foreground.B);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
