using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository {
    class LevelRepository {
        private static LevelRepository _instance;
        public static LevelRepository Instance => _instance ??= new LevelRepository();
        protected LevelRepository() { }

        public async Task<(List<Level>, int)?> RequestLevels(LevelFilter filter) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/levels?{filter}");
            if (json == null) return null;

            var results = json["results"];
            var result = ((ProxyArray) results).Select(Level.FromJson).ToList();

            return (result, json["count"]);
        }

        public async Task<Level> RequestLevel(int id) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/levels/{id}");
            return json == null ? null : Level.FromJson(json);
        }
    }
}