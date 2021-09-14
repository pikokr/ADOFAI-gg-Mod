using Cysharp.Threading.Tasks;
using System.Collections.Specialized;
using UnityEngine.Networking;

namespace ADOFAI_GG.Utils
{
    class NetworkUtil
    {

        public static async UniTask<string> GetTextAsync(string path, NameValueCollection query = null)
        {
            var request = UnityWebRequest.Get(Constants.BASE_URL + path + (query == null ? "" : "?" + query));
            var operation = await request.SendWebRequest();
            return operation.downloadHandler.text;
        }


    }
}
