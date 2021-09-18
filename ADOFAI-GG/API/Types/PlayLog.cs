using System;
using System.Collections;
using System.Globalization;
using System.Threading.Tasks;
using ADOFAI_GG.API.TinyJSON.Types;
using ADOFAI_GG.Utils;
using Steamworks;

namespace ADOFAI_GG.API.Types {
    public class PlayLog {
        public readonly int Id;
        public readonly DateTime TimeStamp;
        public readonly Level Level;
        public readonly Person Player;
        public readonly double RawAccuracy;
        public readonly double Accuracy;
        public readonly int Speed;
        public readonly string URL;
        public readonly bool Accept;
        public readonly double PlayPoint;
        public readonly string Description;

        internal PlayLog(int id, DateTime timeStamp, Level level, Person player, double rawAccuracy, double accuracy,
            int speed, string url, bool accept, double playPoint, string description) {
            Id = id;
            TimeStamp = timeStamp;
            Level = level;
            Player = player;
            RawAccuracy = rawAccuracy;
            Accuracy = accuracy;
            Speed = speed;
            URL = url;
            Accept = accept;
            PlayPoint = playPoint;
            Description = description;
        }
        
        public static async Task<PlayLog> FromJson(Variant obj) {
            if (obj == null) return null;
            var levelId = obj["level"]["id"].ToInt32(CultureInfo.InvariantCulture);
            var playerId = obj["player"]["id"].ToInt32(CultureInfo.InvariantCulture);
            var level = await Request.RequestLevel(obj["level"]["id"].ToInt32(CultureInfo.InvariantCulture));
            var player = await  Request.RequestPerson(obj["player"]["id"].ToInt32(CultureInfo.InvariantCulture));
            var timeString =
                obj["timestamp"].ToString(CultureInfo.InvariantCulture).Split('T');
            var date = timeString[0].Split('-');
            var time = timeString[1].Split(':');
            var timeStamp = new DateTime(
                int.Parse(date[0]),
                int.Parse(date[1]),
                int.Parse(date[2]),
                int.Parse(time[0]),
                int.Parse(time[1]),
                int.Parse(time[2])
            );
            return new PlayLog(
                obj.GetOrNull("id").ToInt32(CultureInfo.InvariantCulture),
                timeStamp,
                level ?? default(Level),
                player ?? default(Person),
                obj.GetOrNull("rawAccuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("accuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("speed")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("url")?.ToString(CultureInfo.InvariantCulture),
                obj.GetOrNull("accept")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
                obj.GetOrNull("playPoint")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("description")?.ToString(CultureInfo.InvariantCulture)
            );
        }
        
        public static IEnumerator FromJson(Variant obj, Func<PlayLog, IEnumerator> callback, Func<IEnumerator> fail = null) {
            IEnumerator Callback(Level level) {
                IEnumerator Callback2(Person player) {
                    var timeString =
                        obj["timestamp"].ToString(CultureInfo.InvariantCulture).Split('T');
                    var date = timeString[0].Split('-');
                    var time = timeString[1].Split(':');
                    var timeStamp = new DateTime(
                        int.Parse(date[0]),
                        int.Parse(date[1]),
                        int.Parse(date[2]),
                        int.Parse(time[0]),
                        int.Parse(time[1]),
                        int.Parse(time[2])
                    );
                    yield return callback(new PlayLog(
                        obj.GetOrNull("id").ToInt32(CultureInfo.InvariantCulture),
                        timeStamp,
                        level ?? default(Level),
                        player ?? default(Person),
                        obj.GetOrNull("rawAccuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                        obj.GetOrNull("accuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                        obj.GetOrNull("speed")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
                        obj.GetOrNull("url")?.ToString(CultureInfo.InvariantCulture),
                        obj.GetOrNull("accept")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
                        obj.GetOrNull("playPoint")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                        obj.GetOrNull("description")?.ToString(CultureInfo.InvariantCulture)
                    ));
                    yield break;
                }
                yield return RequestCo.RequestPerson(obj["player"]["id"].ToInt32(CultureInfo.InvariantCulture), Callback2, fail);
            }

            if (obj == null) {
                fail?.Invoke();
                yield break;
            }
            var levelId = obj["level"]["id"].ToInt32(CultureInfo.InvariantCulture);
            var playerId = obj["player"]["id"].ToInt32(CultureInfo.InvariantCulture);
            yield return RequestCo.RequestLevel(obj["level"]["id"].ToInt32(CultureInfo.InvariantCulture), Callback, fail);
            
        }
    }
}