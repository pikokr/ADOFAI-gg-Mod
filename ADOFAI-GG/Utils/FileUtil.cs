using System.IO;

namespace ADOFAI_GG.Utils
{
    class FileUtil
    {

        public static string GetBasePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Mods", "ADOFAI_GG");
        }

        public static string ToSafeFileName(string fileName)
        {
            return fileName
                .Replace("\\", "")
                .Replace("/", "")
                .Replace(":", "")
                .Replace("*", "")
                .Replace("?", "")
                .Replace("\"", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }

    }
}
