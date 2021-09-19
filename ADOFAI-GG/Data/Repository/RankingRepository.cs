using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository
{
    class RankingRepository
    {
        private static RankingRepository instance;

        public static RankingRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new RankingRepository();
            }
            return instance;
        }

        protected RankingRepository()
        {
        }

        public async Task<(List<Ranking>, int)?> RequestRanking(RankingFilter filter)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/ranking?{filter}");
            if (json == null) return null;
            var result = new List<Ranking>();
            var results = json["results"];

            foreach (var rank in (ProxyArray) results)
            {
                result.Add(await Ranking.FromJson(rank));
            }

            return (result, json["count"]);
        }

    }
}
