using System;
using System.Collections.Generic;

namespace ADOFAI_GG.Domain.Model.Levels
{

    public class LevelTag
    {
        public long id;

        public string name;
    }
    
    public class Level
    {
        public long id;

        public string title;

        public double difficulty;

        public List<string> creators;

        public long songId;

        public string song;

        public List<string> artists;

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

        public List<LevelTag> tags;
    }
}