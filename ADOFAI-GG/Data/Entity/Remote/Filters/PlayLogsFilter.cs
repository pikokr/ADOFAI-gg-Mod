using ADOFAI_GG.Data.Entity.Remote.SortOrder;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public sealed class PlayLogsFilter : SearchFilter {
        public PlayLogsFilter(int offset, int amount) : base(offset, amount) { }

        public int? playerId {
            get => (int?) Filters["playerId"];
            set => Filters["playerId"] = value;
        }

        public int? levelId {
            get => (int?) Filters["levelId"];
            set => Filters["levelId"] = value;
        }

        public PlayLogsSortOrder? sort {
            get => (PlayLogsSortOrder?) Filters["sort"];
            set => Filters["sort"] = value;
        }
        
        public bool? showNotVerified {
            get => (bool?) Filters["showNotVerified"];
            set => Filters["showNotVerified"] = value;
        }
                
        public bool? showDuplicatedPerson {
            get => (bool?) Filters["showDuplicatedPerson"];
            set => Filters["showDuplicatedPerson"] = value;
        }
    }
}