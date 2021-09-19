using TinyJSON;
using TinyJSON.Types;

namespace ADOFAI_GG.Utils {
    public static class JsonUtil {
        public static Variant GetOrNull(this Variant dict, string key) {
            if (dict is ProxyObject obj) {
                return obj.TryGetValue(key, out var value) ? value : null;
            }

            return null;
        }

        public static T Convert<T>(string jsonStr, T instance) where T : class {
            JSON.Load(jsonStr).Populate(instance);
            return instance;
        }
    }
}