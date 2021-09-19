using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADOFAI_GG.Data.Entity.Remote.SortOrder;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public sealed class LevelFilter : SearchFilter {
        public LevelFilter(int offset, int amount) : base(offset, amount) { }

        private List<Tag> _includeTags = new();
        private List<Tag> _excludeTags = new();
        
        public string query {
            get => (string) Filters["query"];
            set => Filters["query"] = value;
        }
        
        public string queryTitle {
            get => (string) Filters["queryTitle"];
            set => Filters["queryTitle"] = value;
        }

        public string queryArtist {
            get => (string) Filters["queryArtist"];
            set => Filters["queryArtist"] = value;
        }

        public string queryCreator {
            get => (string) Filters["queryCreator"];
            set => Filters["queryCreator"] = value;
        }

        public LevelsSortOrder? sort {
            get => (LevelsSortOrder?) Filters["sort"];
            set => Filters["sort"] = value;
        }

        public byte? minDifficulty {
            get => (byte?) Filters["minDifficulty"];
            set => Filters["minDifficulty"] = value;
        }

        public byte? maxDifficulty {
            get => (byte?) Filters["maxDifficulty"];
            set => Filters["maxDifficulty"] = value;
        }

        public double? minBpm {
            get => (double?) Filters["minBpm"];
            set => Filters["minBpm"] = value;
        }

        public double? maxBpm {
            get => (double?) Filters["maxBpm"];
            set => Filters["maxBpm"] = value;
        }

        public int? minTiles {
            get => (int?) Filters["minTiles"];
            set => Filters["minTiles"] = value;
        }

        public int? maxTiles {
            get => (int?) Filters["maxTiles"];
            set => Filters["maxTiles"] = value;
        }

        public bool? showNotVerified {
            get => (bool?) Filters["showNotVerified"];
            set => Filters["showNotVerified"] = value;
        }

        public bool? showCensored {
            get => (bool?) Filters["showCensored"];
            set => Filters["showCensored"] = value;
        }
        
        public Tag[] includeTags {
            get => _includeTags.ToArray();
            set => _includeTags = value.ToList();
        }
                
        public Tag[] excludeTags {
            get => _excludeTags.ToArray();
            set => _excludeTags = value.ToList();
        }

        public override string ToString() {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, object value) in Filters) query.Add(key, $"{value}");
            query.Add("includeTags", string.Join(", ", _includeTags.Select(tag => tag.id)));
            query.Add("excludeTags", string.Join(", ", _excludeTags.Select(tag => tag.id)));
            return query.ToString();
        }
    }
}