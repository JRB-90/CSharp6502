using System.Collections.Generic;

namespace CS6502.UIConsole.Shared
{
    internal class AssetManager
    {
        public AssetManager(string fontName)
        {
            Dictionary<byte, int> asciiToCharMap = new Dictionary<byte, int>();
            
            for (byte i = 32; i <= 126; i++)
            {
                asciiToCharMap.Add(i, i - 32);
            }

            Font =
                BitmapFontLoader.LoadFromEmbeddedFile(
                    "dogica.png",
                    8,
                    8,
                    asciiToCharMap
                );
        }

        public BitmapFont Font { get; }
    }
}
