using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
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
