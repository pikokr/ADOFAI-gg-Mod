using Cysharp.Threading.Tasks;
using MelonLoader;
using MelonLoader.TinyJSON;
using System;
using System.Collections.Specialized;
using System.Net.Http;

namespace ADOFAI_GG.Utils
{
    class NetworkUtil
    {

        public static async UniTask<string> GetTextAsync(string path, NameValueCollection query = null)
        {

            var client = new HttpClient
            {
                BaseAddress = new Uri(Constants.BASE_URL)
            };

            HttpResponseMessage response = client.GetAsync(path + (query == null ? "" : "?" + query)).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }

            return null;
        }


    }
}
