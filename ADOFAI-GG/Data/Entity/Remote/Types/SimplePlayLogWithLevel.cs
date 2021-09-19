using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Entity.Remote.Types
{
    public class SimplePlayLogWithLevel
    {
        public readonly int Id;
        public readonly int LevelId;
        public readonly List<string> Artists;
        public readonly string Title;
        public readonly double Difficulty;
        public readonly int Speed;
        public readonly double RawAccuracy;
        public readonly double Accuracy;
        public readonly double PlayPoint;
        public readonly string URL;

        internal SimplePlayLogWithLevel(int id, int levelId, List<string> artists, string title, 
            double difficulty, int speed, double rawAccuracy, double accuracy, double playPoint, string url)
        {
            Id = id;
            LevelId = levelId;
            Artists = artists;
            Title = title;
            Difficulty = difficulty;
            Speed = speed;
            RawAccuracy = rawAccuracy;
            Accuracy = accuracy;
            PlayPoint = playPoint;
            URL = url;
        }

        public static SimplePlayLogWithLevel FromJson(Variant obj)
        {
            return new SimplePlayLogWithLevel(
                obj["id"],
                obj["levelId"],
                (from artist in obj["artists"] as ProxyArray select artist.ToString()).ToList(), 
                obj["title"], 
                obj["difficulty"], 
                obj["speed"], 
                obj["rawAccuracy"], 
                obj["accuracy"], 
                obj["playPoint"], 
                obj["url"]
            );
        }

    }
}
