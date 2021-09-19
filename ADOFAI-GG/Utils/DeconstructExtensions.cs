using System.Collections.Generic;
using System.Text.RegularExpressions;
using TinyJSON.Types;

namespace ADOFAI_GG.Utils {
	public static class DeconstructExtensions {
		public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, out TKey key,
			out TValue value) {
			key = pair.Key;
			value = pair.Value;
		}

	}
}