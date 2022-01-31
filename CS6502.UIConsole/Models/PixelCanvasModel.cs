using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CS6502.UIConsole.Shared;

namespace CS6502.UIConsole.Models
{
    internal class PixelCanvasModel
    {
        readonly BitmapFont font;

        public PixelCanvasModel(
            int width,
            int height,
            BitmapFont font)
        {
            this.font = font;

            Bitmap =
                new WriteableBitmap(
                    new PixelSize(width, height),
                    new Vector(96, 96),
                    PixelFormat.Bgra8888,
                    AlphaFormat.Opaque
                );

            Reset(0x00);
            DrawFont(0xFF);
        }

        public WriteableBitmap Bitmap { get; }

        private unsafe void Reset(byte color)
        {
            using (var buf = Bitmap.Lock())
            {
                var ptr = (uint*)buf.Address;
                var w = Bitmap.PixelSize.Width;
                var h = Bitmap.PixelSize.Height;

                for (var i = 0; i < w * h; i++)
                {
                    *(ptr + i) = (uint)(0xFF000000 | color << 16 | color << 8 | color);
                }
            }
        }

        private unsafe void DrawFont(byte color)
        {
            using (var buf = Bitmap.Lock())
            {
                var ptr = (uint*)buf.Address;
                var w = Bitmap.PixelSize.Width;
                var h = Bitmap.PixelSize.Height;
                int charsPerCol = Bitmap.PixelSize.Height / 8;
                int charsPerRow = Bitmap.PixelSize.Width / 8;

                for (int cy = 0; cy < charsPerCol; cy++)
                {
                    for (int cx = 0; cx < charsPerRow; cx++)
                    {
                        int pixX = cx * 8;
                        int pixY = cy * 8;
                        byte charIndex = (byte)(((cy * charsPerRow + cx) % (127 - 32)) + 32);
                        FontChar fontChar = font.AsciiToFontChar(charIndex);

                        for (int py = 0; py < 8; py++)
                        {
                            for (int px = 0; px < 8; px++)
                            {
                                int index = (pixY + py) * Bitmap.PixelSize.Width + (pixX + px);
                                if (fontChar.GetPixelValue(py, px) > 0)
                                {
                                    *(ptr + index) = (uint)(0xFF000000 | color << 16 | color << 8 | color);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
