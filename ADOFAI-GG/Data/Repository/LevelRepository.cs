using ADOFAI_GG.Data.Entity.Remote;
using ADOFAI_GG.Utils;
using Cysharp.Threading.Tasks;
using System.Web;
using UnityEngine;

namespace ADOFAI_GG.Data.Repository
{
    class LevelRepository
    {

        private static LevelRepository instance;

        public static LevelRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new LevelRepository();
            }
            return instance;
        }

        protected LevelRepository()
        {
        }

        public async UniTask<LevelSearchResult> Search(int page, int pageSize, string searchQuery, string sort)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("offset", $"{pageSize * page}");
            query.Add("amount", $"{pageSize}");
            query.Add("queryTitle", searchQuery);
            query.Add("queryCreator", searchQuery);
            query.Add("queryArtist", searchQuery);
            query.Add("sort", sort);

            string resultString = await NetworkUtil.GetTextAsync("api/v1/levels", query);

            return JsonUtility.FromJson<LevelSearchResult>(resultString);
        }

    }

}
