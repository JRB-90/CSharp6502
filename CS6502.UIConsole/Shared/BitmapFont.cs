using System.Collections.Generic;

namespace CS6502.UIConsole.Shared
{
    internal class BitmapFont
    {
        public BitmapFont(
            int width,
            int height,
            IReadOnlyList<FontChar> chars)
        {
            Width = width;
            Height = height;

            blankChar =
                new FontChar(
                    width,
                    height,
                    new byte[height * width]
                );

            indexedChars = new Dictionary<byte, FontChar>();

            for (byte i = 0; i <= byte.MaxValue; i++)
            {
                indexedChars[i] = chars[i];
            }
        }

        public BitmapFont(
            int width,
            int height,
            IReadOnlyList<FontChar> chars,
            IDictionary<byte, int> asciiToCharMap)
        {
            Width = width;
            Height = height;

            blankChar =
                new FontChar(
                    width, 
                    height, 
                    new byte[height]
                );

            indexedChars = new Dictionary<byte, FontChar>();

            foreach (var pair in asciiToCharMap)
            {
                indexedChars[pair.Key] = chars[pair.Value];
            }
        }

        public int Width { get; }

        public int Height { get; }

        public FontChar AsciiToFontChar(byte asciiValue)
        {
            if (indexedChars.ContainsKey(asciiValue))
            {
                return indexedChars[asciiValue];
            }
            else
            {
                return blankChar;
            }
        }

        private FontChar blankChar;
        private Dictionary<byte, FontChar> indexedChars;
    }
}
