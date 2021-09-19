using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository {
    class RankingRepository {
        private static RankingRepository _instance;
        public static RankingRepository Instance => _instance ??= new RankingRepository();
        protected RankingRepository() { }

        public async Task<(List<Ranking>, int)?> RequestRanking(RankingFilter filter) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/ranking?{filter}");
            if (json == null) return null;
            var results = json["results"];

            var result = ((ProxyArray) results).Select(Ranking.FromJson).ToList();

            return (result, json["count"]);
        }
    }
}