using ADOFAI_GG.Data.Entity.Remote.SortOrder;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public sealed class PlayLogsFilter : SearchFilter {
        public PlayLogsFilter(int offset, int amount) : base(offset, amount) { }

        private int? playerId {
            get => (int?) filters["playerId"];
            set => filters["playerId"] = value;
        }

        private int? levelId {
            get => (int?) filters["levelId"];
            set => filters["levelId"] = value;
        }

        private PlayLogsSortOrder? sort {
            get => (PlayLogsSortOrder?) filters["sort"];
            set => filters["sort"] = value;
        }
        
        private bool? showNotVerified {
            get => (bool?) filters["showNotVerified"];
            set => filters["showNotVerified"] = value;
        }
                
        private bool? showDuplicatedPerson {
            get => (bool?) filters["showDuplicatedPerson"];
            set => filters["showDuplicatedPerson"] = value;
        }
        
        public PlayLogsFilter PlayerId(int value) {
            playerId = value;
            return this;
        }
        
        public PlayLogsFilter LevelId(int value) {
            levelId = value;
            return this;
        }
        
        public PlayLogsFilter Sort(PlayLogsSortOrder value) {
            sort = value;
            return this;
        }
        
        public PlayLogsFilter ShowNotVerified(bool value) {
            showNotVerified = value;
            return this;
        }
        
        public PlayLogsFilter ShowDuplicatedPerson(bool value) {
            showDuplicatedPerson = value;
            return this;
        }
    }
}