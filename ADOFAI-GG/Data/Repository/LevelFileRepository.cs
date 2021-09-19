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

namespace ADOFAI_GG.Data.Repository {
	class LevelFileRepository {
		private static LevelFileRepository _instance;
		public static LevelFileRepository Instance => _instance ??= new LevelFileRepository();
		private readonly string _baseLevelPath;

		protected LevelFileRepository() {
			_baseLevelPath = Path.Combine(FileUtil.BasePath, "Levels");
			if (!Directory.Exists(_baseLevelPath)) {
				RDDirectory.CreateDirectory(_baseLevelPath);
			}
		}

		private string GetLevelPath(int id) => Path.Combine(_baseLevelPath, id.ToString());
		
		public async Task<bool> DownloadLevel(Level level) {
			string url = NetworkUtil.GetDirectDownloadUrl(level.Download);
			int id = level.Id;

			var client = UnityWebRequest.Get(url);
			await client.SendWebRequest();

			MelonLogger.Msg($"Response: {client.responseCode}");
			if (client.isHttpError || client.isNetworkError) {
				MelonLogger.Msg($"Download failed");
				return false;
			}

			byte[] result = client.downloadHandler.data;

			MelonLogger.Msg("Download success");

			string levelPath = GetLevelPath(id);
			string levelFilePath = levelPath.TrimEnd('/').TrimEnd('\\') + ".zip";
			if (Directory.Exists(levelPath)) Directory.Delete(levelPath, true);

			RDFile.WriteAllBytes(levelFilePath, result);

			RDDirectory.CreateDirectory(levelPath);

			try {
				ZipUtil.Unzip(levelFilePath, levelPath);
			} catch (Exception ex) {
				MelonLogger.Error("Unzip failed: " + ex);
				Directory.Delete(levelPath, true);
				return false;
			}

			MelonLogger.Msg("Unzip success");
			RDFile.Delete(levelFilePath);
			return true;
		}

		public void DeleteLevel(int id) {
			Directory.Delete(GetLevelPath(id), true);
		}

		public bool IsLevelExists(int id) {
			return Directory.Exists(GetLevelPath(id));
		}

		public async Task<bool> LoadLevel(int id) {
			string dataPathFromURL = GetLevelPath(id);

			string text2 = FindAdofaiLevelOnDirectory(dataPathFromURL);
			if (text2 != null) {
				GCS.customLevelIndex = 0;
				GCS.speedTrialMode = false;
				GCS.customLevelPaths = CustomLevel.GetWorldPaths(text2, false, true);
				GCS.customLevelIndex = 0;
				GCS.standaloneLevelMode = true;
				await SceneManager.LoadSceneAsync(GCNS.sceneEditor);
				return true;
			} else {
				Directory.Delete(dataPathFromURL, true);
				RDFile.Delete(dataPathFromURL);
				return false;
			}
		}
		
		private static string FindAdofaiLevelOnDirectory(string path) =>
			Directory.GetFiles(path, "*.adofai", SearchOption.AllDirectories)
				.First(directory => !Path.GetFileName(directory).StartsWith("."));

		public static bool IsDownloadable(string url) =>
			url != null &&
			!url.Contains("cdn.discordapp.com/attachments") &&
			!url.Contains("www.mediafire.com/file/");
	}
}