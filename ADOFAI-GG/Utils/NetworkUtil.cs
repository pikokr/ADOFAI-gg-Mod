using Cysharp.Threading.Tasks;
using MelonLoader;
using System.Collections.Specialized;
using UnityEngine.Networking;

namespace ADOFAI_GG.Utils
{
    class NetworkUtil
    {

        public static async UniTask<string> GetTextAsync(string path, NameValueCollection query = null)
        {
            var request = UnityWebRequest.Get(Constants.BASE_URL + path + (query == null ? "" : "?" + query));

            MelonLogger.Msg("Get");
            var operation = await request.SendWebRequest();
            MelonLogger.Msg("result");
            return operation.downloadHandler.text;
        }


    }
}
