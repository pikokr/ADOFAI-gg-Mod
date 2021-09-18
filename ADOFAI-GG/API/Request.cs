using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ADOFAI_GG.Data.Entity.Remote.Filters;
using TinyJSON;
using TinyJSON.Types;
using ADOFAI_GG.Data.Entity.Remote.Types;
using MelonLoader;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data {
    public static class Request {
        public const string RequestURL = "https://api.adofai.gg:9200/";
        public static string LastError;

        public static async Task<(List<Level>, int)?> RequestLevels(LevelFilter filter) {
            var json = await _request($"api/v1/levels?{filter}");
            if (json == null) return null;
            var result = new List<Level>();
            var results = json["results"];

            foreach (var level in (ProxyArray) results) {
                result.Add(Level.FromJson(level));
            }

            return (result, json["count"]);
        }

        public static async Task<Level> RequestLevel(int id) {
            var json = await _request($"api/v1/levels/{id}");
            if (json == null) return null;
            return Level.FromJson(json);
        }

        public static async Task<(List<Person>, int)?> RequestPeople(PersonFilter filter) {
            var json = await _request($"api/v1/person?{filter}");
            if (json == null) return null;
            var result = new List<Person>();
            var results = json["results"];

            foreach (var person in (ProxyArray) results) {
                result.Add(Person.FromJson(person));
            }

            return (result, json["count"]);
        }

        public static async Task<Person> RequestPerson(int id) {
            var json = await _request($"api/v1/person/{id}");
            if (json == null) return null;
            return Person.FromJson(json);
        }


        public static async Task<(List<Ranking>, int)?> RequestRanking(RankingFilter filter) {
            var json = await _request($"api/v1/ranking?{filter}");
            if (json == null) return null;
            var result = new List<Ranking>();
            var results = json["results"];

            foreach (var rank in (ProxyArray) results) {
                result.Add(await Ranking.FromJson(rank));
            }

            return (result, json["count"]);
        }

        public static async Task<(List<PlayLog>, int)?> RequestPlayLogs(PlayLogsFilter filter) {
            var json = await _request($"api/v1/playLogs?{filter}");
            if (json == null) return null;
            var result = new List<PlayLog>();
            var results = json["results"];

            foreach (var playlog in (ProxyArray) results) {
                result.Add(await PlayLog.FromJson(playlog));
            }

            return (result, json["count"]);
        }

        public static async Task<PlayLog> RequestPlayLog(int id) {
            var json = await _request($"api/v1/playLogs/{id}");
            if (json == null) return null;
            return await PlayLog.FromJson(json);
        }

        public static async Task<Dictionary<TagType, Dictionary<int, string>>> RequestTags() {
            var json = await _request($"api/v1/tags");
            if (json == null) return null;
            var results = json["results"];
            MelonLogger.Msg("tags new list");
            var allTags = new List<(int, TagType, string)>();

            foreach (var tag in (ProxyArray) results) {
                var id = tag["id"].ToInt32(CultureInfo.InvariantCulture);
                var typeName = tag["type"].ToString(CultureInfo.InvariantCulture);
                var name = tag["name"].ToString(CultureInfo.InvariantCulture);

                if (Enum.TryParse<TagType>(typeName, out var tagType)) {
                    allTags.Add((id, tagType, name));
                }
            }

            MelonLogger.Msg("tags new dictionary");
            var result = new Dictionary<TagType, Dictionary<int, string>>();
            foreach (int tag in Enum.GetValues(typeof(TagType))) {
                var tags = new Dictionary<int, string>();
                foreach (var tuple in allTags.Where(tuple => tuple.Item2 == (TagType) tag)) {
                    tags[tuple.Item1] = tuple.Item3;
                    MelonLogger.Msg($"Loaded Tag {tuple.Item1}: {tuple.Item3}");
                }

                result.Add((TagType) tag, tags);
            }

            MelonLogger.Msg("success tag get");

            return result;
        }

        private static async Task<Variant> _request(string url) {
            MelonLogger.Msg(url);

            string content = await NetworkUtil.GetTextAsync(url);
            MelonLogger.Msg("Failed fetching");

            if (content != null)
            {
                return JSON.Load(content);
            }

            MelonLogger.Msg("Failed fetching");
            return null;
        }
    }
}