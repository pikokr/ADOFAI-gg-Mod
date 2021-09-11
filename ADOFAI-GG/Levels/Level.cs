using System;

namespace ADOFAI_GG.Levels
{
    [Serializable]
    public class LevelTag
    {
        public long id;

        public string name;
    }
    
    [Serializable]
    public class Level
    {
        public long id;

        public string title;

        public double difficulty;

        public string[] creators;

        public long songId;

        public string song;

        public string[] artists;

        public double minBpm;
        
        public double maxBpm;

        public long tiles;

        public int comments;

        public int likes;

        public bool epilepsyWarning;

        public bool censored;

        public string video;

        public string download;

        public string workshop;

        public LevelTag[] tags;
    }
}