using Cysharp.Threading.Tasks;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TinyJSON;
using TinyJSON.Types;
using UnityEngine.Networking;

namespace ADOFAI_GG.Utils
{
    class NetworkUtil
    {
        
        private static readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri(Constants.BASE_URL)
        };

        public static async Task<string> GetTextAsync(string path, NameValueCollection query = null)
        {
            //return await GetTextAsyncHttpClient(path, query);
            return await GetTextAsyncUnityWebRequest(path, query);
        }

        public static async Task<string> GetTextAsyncHttpClient(string path, NameValueCollection query = null)
        {
            MelonLoader.MelonLogger.Msg("GetTextAsync GetAsync " + path + (query == null ? "" : "?" + query));
            HttpResponseMessage response = await client.GetAsync(path + (query == null ? "" : "?" + query));
            MelonLoader.MelonLogger.Msg("GetTextAsync response");
            if (response.IsSuccessStatusCode)
            {
                MelonLoader.MelonLogger.Msg("GetTextAsync success");
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }

            MelonLoader.MelonLogger.Msg("GetTextAsync fail");
            return null;
        }

        public static async Task<string> GetTextAsyncUnityWebRequest(string path, NameValueCollection query = null)
        {
            var client = new UnityWebRequest(Constants.BASE_URL + path + (query == null ? "" : "?" + query));
            client.downloadHandler = new DownloadHandlerBuffer();
            await client.SendWebRequest();

            if (client.isHttpError || client.isNetworkError)
            {
                MelonLoader.MelonLogger.Error("Failed fetching " + path);
                return null;
            }

            var content = client.downloadHandler.text;
            return content;
        }

        public static async Task<Variant> GetJsonAsync(string path, NameValueCollection query = null)
        {
            string result = await GetTextAsync(path, query);
            if (result == null)
            {
                return null;
            }

            Variant variant = JSON.Load(result);
            return variant;
        }

        public static string GetDirectDownloadUrl(string url)
        {
            if (url.Contains("drive.google.com/file/d/"))
            {
                var pattern = "drive.google.com/file/d/.+?/";
                var match = Regex.Match(url, pattern);
                var id = match.Groups[0].Value.Substring(24).Replace("/", "");
                return $"https://drive.google.com/uc?export=download&id={id}";
            }

            if (url.Contains("www.mediafire.com/file/"))
            {

            }

            return url;
        }


    }
}
