using System.IO;
using UnityEngine;

namespace ADOFAI_GG
{
    public class Assets
    {
        public static AssetBundle Bundle;
        
        public static void Init()
        {
            Bundle = AssetBundle.LoadFromMemory(ReadFully(
                typeof(AdofaiGG).Assembly.GetManifestResourceStream("ADOFAI_GG.Resources.assets.bundle")));
            Bundle.GetAllScenePaths();
        }
        
        private static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[81920];
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}