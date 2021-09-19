using ADOFAI_GG.Data.Entity.Remote;
using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using TinyJSON.Types;

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

        public async Task<LevelSearchResult> Search(int page, int pageSize, string searchQuery, string sort)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            query.Add("offset", $"{pageSize * page}");
            query.Add("amount", $"{pageSize}");
            query.Add("queryTitle", searchQuery);
            query.Add("queryCreator", searchQuery);
            query.Add("queryArtist", searchQuery);
            query.Add("sort", sort);
            string resultString = await NetworkUtil.GetTextAsync("api/v1/levels", query);
            
            return JsonUtil.Convert(resultString, new LevelSearchResult());
        }

        public async Task<(List<Level>, int)?> RequestLevels(LevelFilter filter)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/levels?{filter}");
            if (json == null) return null;
            

            var result = new List<Level>();
            var results = json["results"];
            foreach (var level in (ProxyArray) results)
            {
                result.Add(Level.FromJson(level));
            }

            return (result, json["count"]);
        }

        public async Task<Level> RequestLevel(int id)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/levels/{id}");
            if (json == null) return null;
            return Level.FromJson(json);
        }

    }

}
