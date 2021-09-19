using System;
using System.Linq;
using System.IO;
using ADOFAI_GG.Utils;
using MelonLoader;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using ADOFAI_GG.Data.Entity.Remote.Types;

namespace ADOFAI_GG.Data.Repository
{
    class LevelFileRepository
    {

        private static LevelFileRepository instance;

        public static LevelFileRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new LevelFileRepository();
            }
            return instance;
        }

		private readonly string baseLevelPath;

		protected LevelFileRepository()
        {
			baseLevelPath = Path.Combine(FileUtil.GetBasePath(), "Levels");
			if (!Directory.Exists(baseLevelPath))
			{
				RDDirectory.CreateDirectory(baseLevelPath);
			}

		}


		private string FindAdofaiLevelOnDirectory(string path)
		{
			return Directory.GetFiles(path, "*.adofai", SearchOption.AllDirectories)
				.Where(directory => !Path.GetFileName(directory).StartsWith("."))
				.First();
		}

		private string GetLevelPath(int id)
		{
			return Path.Combine(baseLevelPath, id.ToString());
		}

		public async Task<bool> DownloadLevel(Level level)
		{
			String url = NetworkUtil.GetDirectDownloadUrl(level.Download);
			int id = level.Id;

			var client = UnityWebRequest.Get(url);
			await client.SendWebRequest();

			MelonLogger.Msg($"Response: {client.responseCode}");
			if (client.isHttpError || client.isNetworkError)
			{
				MelonLogger.Msg($"Download failed");
				return false;
			}

			var result = client.downloadHandler.data;

			MelonLogger.Msg("Download success");

			string levelPath = GetLevelPath(id);
			var levelFilePath = levelPath.TrimEnd('/').TrimEnd('\\') + ".zip";
			if (Directory.Exists(levelPath)) Directory.Delete(levelPath, true);

			RDFile.WriteAllBytes(levelFilePath, result);

			RDDirectory.CreateDirectory(levelPath);

			try
            {
				ZipUtil.Unzip(levelFilePath, levelPath);
			}
			catch (Exception ex)
			{
				MelonLogger.Error("Unzip failed: " + ex);
				Directory.Delete(levelPath, true);
				return false;
			}

			MelonLogger.Msg("Unzip success");
			RDFile.Delete(levelFilePath);
			return true;
		}

		public void DeleteLevel(int id)
		{
			Directory.Delete(GetLevelPath(id), true);
		}

		public bool IsLevelExists(int id)
		{
			return Directory.Exists(GetLevelPath(id));
		}

		public async Task<bool> LoadLevel(int id)
		{
			var dataPathFromURL = GetLevelPath(id);

			string text2 = FindAdofaiLevelOnDirectory(dataPathFromURL);
			if (text2 != null)
			{
				GCS.customLevelIndex = 0;
				GCS.speedTrialMode = false;
				GCS.customLevelPaths = CustomLevel.GetWorldPaths(text2, false, true);
				GCS.customLevelIndex = 0;
				GCS.standaloneLevelMode = true;
				await SceneManager.LoadSceneAsync(GCNS.sceneEditor);
				return true;
			}
			else
			{
				Directory.Delete(dataPathFromURL, true);
				RDFile.Delete(dataPathFromURL);
				return false;
			}
		}

        public bool isDownloadable(string url)
        {
			return url != null &&
				!url.Contains("cdn.discordapp.com/attachments") &&
				!url.Contains("www.mediafire.com/file/");
        }

    }
}
