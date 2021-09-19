using System.IO;

namespace ADOFAI_GG.Utils {
    public static class FileUtil {
        public static string BasePath => Path.Combine(Directory.GetCurrentDirectory(), "Mods", "ADOFAI_GG");

        public static string ToSafeFileName(string fileName) =>
            fileName.Replace("\\", "")
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