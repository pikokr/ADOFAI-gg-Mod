using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository
{
    class PlayLogRepository
    {

        private static PlayLogRepository instance;

        public static PlayLogRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new PlayLogRepository();
            }
            return instance;
        }

        protected PlayLogRepository()
        {
        }

        public async Task<(List<PlayLog>, int)?> RequestPlayLogs(PlayLogsFilter filter)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/playLogs?{filter}");
            if (json == null) return null;
            var result = new List<PlayLog>();
            var results = json["results"];

            foreach (var playlog in (ProxyArray) results)
            {
                result.Add(await PlayLog.FromJson(playlog));
            }

            return (result, json["count"]);
        }

        public async Task<PlayLog> RequestPlayLog(int id)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/playLogs/{id}");
            if (json == null) return null;
            return await PlayLog.FromJson(json);
        }
    }
}
