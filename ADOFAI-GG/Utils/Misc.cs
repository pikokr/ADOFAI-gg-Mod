using System.Collections.Generic;
using System.Text.RegularExpressions;
using TinyJSON.Types;

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

		public static string LinkToDirect(this string url) {
			if (url.Contains("drive.google.com/file/d/")) {
				var pattern = "drive.google.com/file/d/.+?/";
				var match = Regex.Match(url, pattern);
				var id = match.Groups[0].Value.Substring(24).Replace("/", "");
				return $"https://drive.google.com/uc?export=download&id={id}";
			}

			if (url.Contains("www.mediafire.com/file/")) {
				
			}

			return url;
		}
	}
}