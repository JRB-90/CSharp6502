using System.Reflection;

namespace CS6502.ASM
{
    public static class EmbeddedFileLoader
    {
        const string RES_FOLDER_PATH = "CS6502.ASM.Compiled.";

        public static byte[] LoadCompiledBinFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = RES_FOLDER_PATH + name + ".bin";

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
                    throw new FileNotFoundException($"Could not find embedded bin file: {name}");
                }
            }
        }

        public static string LoadBenchmarkCsvFile(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = RES_FOLDER_PATH + name + ".csv";

            using (Stream? stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Could not find embedded csv file: {name}");
                }
            }
        }
    }
}