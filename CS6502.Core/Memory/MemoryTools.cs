using System.IO;

namespace CS6502.Core
{
    public static class MemoryTools
    {
        public static byte[] LoadDataFromFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Failed to load memory data, file does not exist");
            }

            string ext = Path.GetExtension(path).ToUpper();
            if (ext == null)
            {
                throw new FileNotFoundException("Failed to load memory data, cannot read file extension");
            }

            switch (ext)
            {
                case ".BIN":
                    return LoadDataFromBinFile(path);
                default:
                    throw new FileNotFoundException("Failed to load memory data, file type not supported");
            }
        }

        private static byte[] LoadDataFromBinFile(string path)
        {
            byte[] data = File.ReadAllBytes(path);

            return data;
        }

        public static void Populate<T>(this T[] arr, T value)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = value;
            }
        }
    }
}
