using MelonLoader.TinyJSON;

namespace ADOFAI_GG.Utils
{
    class JsonUtil
    {

        public static T Convert<T>(string jsonStr, T instance) where T : class
        {
            JSON.Load(jsonStr).Populate(instance);
            return instance;
        }

    }
}
