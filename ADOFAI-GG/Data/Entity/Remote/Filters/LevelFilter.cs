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

        private List<Tag> includeTags = new();
        private List<Tag> excludeTags = new();

        private string queryTitle {
            get => (string) filters["queryTitle"];
            set => filters["queryTitle"] = value;
        }

        private string queryArtist {
            get => (string) filters["queryArtist"];
            set => filters["queryArtist"] = value;
        }

        private string queryCreator {
            get => (string) filters["queryCreator"];
            set => filters["queryCreator"] = value;
        }

        private LevelsSortOrder? sort {
            get => (LevelsSortOrder?) filters["sort"];
            set => filters["sort"] = value;
        }

        private byte? minDifficulty {
            get => (byte?) filters["minDifficulty"];
            set => filters["minDifficulty"] = value;
        }

        private byte? maxDifficulty {
            get => (byte?) filters["maxDifficulty"];
            set => filters["maxDifficulty"] = value;
        }

        private double? minBPM {
            get => (double?) filters["minBpm"];
            set => filters["minBpm"] = value;
        }

        private double? maxBPM {
            get => (double?) filters["maxBpm"];
            set => filters["maxBpm"] = value;
        }

        private int? minTiles {
            get => (int?) filters["minTiles"];
            set => filters["minTiles"] = value;
        }

        private int? maxTiles {
            get => (int?) filters["maxTiles"];
            set => filters["maxTiles"] = value;
        }

        private bool? showNotVerified {
            get => (bool?) filters["showNotVerified"];
            set => filters["showNotVerified"] = value;
        }

        private bool? showCensored {
            get => (bool?) filters["showCensored"];
            set => filters["showCensored"] = value;
        }

        public LevelFilter QueryTitle(string value) {
            queryTitle = value;
            return this;
        }

        public LevelFilter QueryArtist(string value) {
            queryArtist = value;
            return this;
        }

        public LevelFilter QueryCreator(string value) {
            queryCreator = value;
            return this;
        }

        public LevelFilter Sort(LevelsSortOrder value) {
            sort = value;
            return this;
        }

        public LevelFilter MinDifficulty(byte value) {
            minDifficulty = value;
            return this;
        }

        public LevelFilter MaxDifficulty(byte value) {
            maxDifficulty = value;
            return this;
        }

        public LevelFilter MinBPM(double value) {
            minBPM = value;
            return this;
        }

        public LevelFilter MaxBPM(double value) {
            maxBPM = value;
            return this;
        }

        public LevelFilter MinTiles(int value) {
            minTiles = value;
            return this;
        }

        public LevelFilter MaxTiles(int value) {
            maxTiles = value;
            return this;
        }

        public LevelFilter ShowNotVerified(bool value) {
            showNotVerified = value;
            return this;
        }

        public LevelFilter ShowCensored(bool value) {
            showCensored = value;
            return this;
        }

        public LevelFilter Include(params Tag[] value) {
            foreach (var tag in value) {
                includeTags.Add(tag);
            }

            return this;
        }
        
        public LevelFilter Exclude(params Tag[] value) {
            foreach (var tag in value) {
                excludeTags.Add(tag);
            }

            return this;
        }

        public LevelFilter Custom(string key, object value) {
            filters[key] = value;
            return this;
        }

        public override string ToString() {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, object value) in filters) query.Add(key, $"{value}");
            query.Add("includeTags", string.Join(", ", includeTags.Select(tag => tag.id)));
            query.Add("excludeTags", string.Join(", ", excludeTags.Select(tag => tag.id)));
            return query.ToString();
        }
    }
}