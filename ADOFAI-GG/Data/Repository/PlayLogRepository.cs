using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository {
    class PlayLogRepository {
        private static PlayLogRepository _instance;
        public static PlayLogRepository Instance => _instance ??= new PlayLogRepository();
        protected PlayLogRepository() { }

        public async Task<(List<PlayLog>, int)?> RequestPlayLogs(PlayLogsFilter filter) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/playLogs?{filter}");
            if (json == null) return null;
            var results = json["results"];

            var result = ((ProxyArray) results).Select(PlayLog.FromJson).ToList();

            return (result, json["count"]);
        }

        public async Task<PlayLog> RequestPlayLog(int id) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/playLogs/{id}");
            return json == null ? null : PlayLog.FromJson(json);
        }
    }
}