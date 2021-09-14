using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ADOFAI_GG.API;
using ADOFAI_GG.API.Filters;
using ADOFAI_GG.API.TinyJSON.Types;
using ADOFAI_GG.API.Types;
using ADOFAI_GG.Scenes;
using MelonLoader;
using RDTools;
using Steamworks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ADOFAI_GG.Utils {
	public static class Misc {
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key,
			out TValue value) {
			key = pair.Key;
			value = pair.Value;
		}

		public static Variant GetOrNull(this Variant dict, string key) {
			if (dict is ProxyObject obj) {
				return obj.TryGetValue(key, out var value) ? value : null;
			}

			return null;
		}

		public static string GoogleDriveToDirect(this string url) {
			if (url.Contains("drive.google.com/file/d/")) {
				var pattern = "drive.google.com/file/d/.+?/";
				var match = Regex.Match(url, pattern);
				var id = match.Groups[0].Value.Substring(24).Replace("/", "");
				return $"https://drive.google.com/uc?export=download&id={id}";
			}

			return url;
		}
	}
}