using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Linq;
using ADOFAI_GG.API.TinyJSON.Types;
using ADOFAI_GG.Scenes;
using ADOFAI_GG.Utils;
using MelonLoader;
using RDTools;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ADOFAI_GG.API.Types {
	public class Level {
		public readonly int Id;
		public readonly string Title;
		public readonly double Difficulty;
		public readonly string Description;
		public readonly string Video;
		public readonly string Download;
		public readonly string Workshop;
		public readonly bool EpilepsyWarning;
		public readonly bool NSFW;
		public readonly string[] Creators;
		public readonly int SongId;
		public readonly string Song;
		public readonly string[] Artists;
		public readonly double MinBPM;
		public readonly double MaxBPM;
		public readonly int Tiles;
		public readonly int Comments;
		public readonly int Likes;
		public readonly Tag[] Tags;

		public Level(int id, string title, double difficulty, string description, string video, string download,
			string workshop, bool epilepsyWarning, bool nsfw, string[] creators, int songId, string song,
			string[] artists, double minBpm, double maxBpm, int tiles, int comments, int likes, Tag[] tags) {
			Id = id;
			Title = title;
			Difficulty = difficulty;
			Description = description;
			Video = video;
			Download = download;
			Workshop = workshop;
			EpilepsyWarning = epilepsyWarning;
			NSFW = nsfw;
			Creators = creators;
			SongId = songId;
			Song = song;
			Artists = artists;
			MinBPM = minBpm;
			MaxBPM = maxBpm;
			Tiles = tiles;
			Comments = comments;
			Likes = likes;
			Tags = tags;
		}


		public static Level FromJson(Variant obj) {
			var tags = (from tag in (ProxyArray) obj["tags"]
				select new Tag(TagType.Level, tag["id"].ToInt32(CultureInfo.InvariantCulture))).ToArray();

			return new Level(
				obj.GetOrNull("id")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("title")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("difficulty")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("description")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("video")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("download")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("workshop")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("epilepsyWarning")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
				obj.GetOrNull("nsfw")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
				obj.GetOrNull("creators")?.Make<string[]>(),
				obj.GetOrNull("songId")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("song")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("artists")?.Make<string[]>(),
				obj.GetOrNull("minBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("maxBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("tiles")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("comments")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("likes")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				tags
			);
		}

		private static string FindAdofaiLevelOnDirectory(string path) {
			string[] files = Directory.GetFiles(path, "*.adofai", SearchOption.AllDirectories);
			if (files.Length == 0) {
				return null;
			}

			string s = null;
			for (int i = 0; i < files.Length; i++) {
				if (!Path.GetFileName(files[i]).StartsWith(".")) {
					s = files[i];
					MonoBehaviour.print("selected file: " + s);
					break;
				}
			}

			if (s == null) {
				MonoBehaviour.print("was null");
				return null;
			}

			return s;
		}

		private static string GetDataPathFromURL(string url) {
			int num = url.LastIndexOf('/') + 1;
			string path = url.Substring(num, url.Length - num);
			string text = Path.Combine(Directory.GetCurrentDirectory(), "Mods", "ADOFAI_GG", "Levels");
			if (!Directory.Exists(text)) {
				RDDirectory.CreateDirectory(text);
			}

			string text2 = Path.Combine(text, path).Replace("?", "").Replace("=", "");
			RDBaseDll.printem(text2);
			return text2;
		}
		public IEnumerator DownloadLevel() {
			var url = Download.GoogleDriveToDirect();
			var id = Id;
			MelonLogger.Msg(url);
			Debug.Log(url);
			var client = UnityWebRequest.Get(url);
			yield return client.SendWebRequest();

			MelonLogger.Msg($"Response: {client.responseCode}");
			if (client.isHttpError || client.isNetworkError) {
				MelonLogger.Msg($"Download failed");
				yield return LevelsScene.Instance.Refresh();
				yield break;
			}

			var result = client.downloadHandler.data;

			MelonLogger.Msg("Download success");

			string dataPathFromURL = GetDataPathFromURL(id.ToString());
			if (RDFile.Exists(dataPathFromURL)) {
				RDFile.Delete(dataPathFromURL);
			}

			var filePath = dataPathFromURL.TrimEnd('/').TrimEnd('\\') + ".zip";
			RDFile.WriteAllBytes(filePath, result);
			RDBaseDll.printem(dataPathFromURL + " " + dataPathFromURL);
			if (Directory.Exists(dataPathFromURL)) {
				Directory.Delete(dataPathFromURL, true);
			}

			RDDirectory.CreateDirectory(dataPathFromURL);
			bool flag = false;
			try {
				ZipUtil.Unzip(filePath, dataPathFromURL);
				flag = true;
			} catch (Exception ex) {
				MelonLogger.Msg("Unzip failed: " + ex);
			}

			if (!flag) {
				Directory.Delete(dataPathFromURL, true);
				yield break;
			}

			MelonLogger.Msg("Unzip success");
			RDFile.Delete(filePath);
			yield return LevelsScene.Instance.Refresh();
		}

		public void DeleteLevel() {
			string dataPathFromURL = GetDataPathFromURL(Id.ToString());
			Directory.Delete(dataPathFromURL, true);
			LevelsScene.Instance.StartCoroutine(LevelsScene.Instance.Refresh());
		}

		public bool CheckLevelExists() {
			string dataPathFromURL = GetDataPathFromURL(Id.ToString());
			return Directory.Exists(dataPathFromURL);
		}

		public IEnumerator LoadLevel() {
			var dataPathFromURL = GetDataPathFromURL(Id.ToString());
			yield return SceneManager.LoadSceneAsync("scnCLS", LoadSceneMode.Additive);

			string text2 = FindAdofaiLevelOnDirectory(dataPathFromURL);
			if (text2 != null) {
				GCS.customLevelIndex = 0;
				GCS.speedTrialMode = false;
				scrController.instance.audioManager.StopLoadingMP3File();
				scrController.instance.LoadCustomWorld(text2);
			} else {
				Directory.Delete(dataPathFromURL, true);
				RDFile.Delete(dataPathFromURL);
			}
		}
	}
}