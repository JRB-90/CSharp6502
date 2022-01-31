using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace CS6502.UIConsole.Shared
{
    internal static class BitmapFontLoader
    {
        const string ASSET_FOLDER_PATH = "CS6502.UIConsole.Assets";

        public static BitmapFont LoadFromEmbeddedFile(
            string name,
            int charWidth,
            int charHeight)
        {
            var bytes =
                LoadEmbeddedFileData(
                    ASSET_FOLDER_PATH,
                    name
                );

            IReadOnlyList<FontChar> chars =
                LoadFromBytes(
                    bytes,
                    charWidth,
                    charHeight
                );

            return
                new BitmapFont(
                    charWidth,
                    charHeight,
                    chars
                );
        }

        public static BitmapFont LoadFromEmbeddedFile(
            string name,
            int charWidth,
            int charHeight,
            IDictionary<byte, int> asciiToCharMap)
        {
            var bytes = 
                LoadEmbeddedFileData(
                    ASSET_FOLDER_PATH,
                    name
                );

            IReadOnlyList<FontChar> chars =
                LoadFromBytes(
                    bytes,
                    charWidth,
                    charHeight
                );

            return
                new BitmapFont(
                    charWidth,
                    charHeight,
                    chars,
                    asciiToCharMap
                );
        }

        private static byte[] LoadEmbeddedFileData(
            string resFolderPath,
            string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = resFolderPath + "." + name;

            using (Stream? stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        var bytes = reader.ReadBytes((int)reader.BaseStream.Length);

                        return bytes;
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Could not find embedded file: {name}");
                }
            }
        }

        private static IReadOnlyList<FontChar> LoadFromBytes(
            byte[] bytes,
            int charWidth,
            int charHeight)
        {
            var image = Image.Load(bytes);
            var charsPerRow = image.Width / 8;
            var charsPerColumn = image.Height / 8;
            List<FontChar> chars = new List<FontChar>();

            for (int cy = 0; cy < charsPerColumn; cy++)
            {
                for (int cx = 0; cx < charsPerRow; cx++)
                {
                    int charX = 8 * cx;
                    int charY = 8 * cy;
                    var charData = new byte[8];

                    for (int px = 0; px < 8; px++)
                    {
                        for (int py = 0; py < 8; py++)
                        {
                            byte r = image[px + charX, py + charY].R;
                            byte g = image[px + charX, py + charY].G;
                            byte b = image[px + charX, py + charY].B;
                            bool isCharPix =
                                r > 127 ||
                                g > 127 ||
                                b > 127;

                            if (isCharPix)
                            {
                                charData[py] |= (byte)(1 << px);
                            }
                        }
                    }

                    chars.Add(
                        new FontChar(
                            charWidth,
                            charHeight,
                            charData
                        )
                    );
                }
            }

            return chars;
        }
    }
}
