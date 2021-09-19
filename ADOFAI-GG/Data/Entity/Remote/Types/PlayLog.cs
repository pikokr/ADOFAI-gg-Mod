using System;
using System.Globalization;
using System.Threading.Tasks;
using TinyJSON.Types;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data.Entity.Remote.Types {
    public class PlayLog {
        public readonly int Id;
        public readonly DateTime TimeStamp;
        public readonly IdNameDto Level;
        public readonly IdNameDto Player;
        public readonly double RawAccuracy;
        public readonly double Accuracy;
        public readonly int Speed;
        public readonly string URL;
        public readonly bool Accept;
        public readonly double PlayPoint;
        public readonly string Description;

        internal PlayLog(int id, DateTime timeStamp, IdNameDto level, IdNameDto player, double rawAccuracy,
            double accuracy,
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

        public static PlayLog FromJson(Variant obj) {
            if (obj == null) return null;

            var level = IdNameDto.FromJson(obj["level"]);
            var player = IdNameDto.FromJson(obj["player"]);

            var timeString = obj["timestamp"].ToString(CultureInfo.InvariantCulture).Split('T');
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
                level ?? default,
                player ?? default,
                obj.GetOrNull("rawAccuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("accuracy")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("speed")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("url")?.ToString(CultureInfo.InvariantCulture),
                obj.GetOrNull("accept")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
                obj.GetOrNull("playPoint")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("description")?.ToString(CultureInfo.InvariantCulture)
            );
        }
    }
}