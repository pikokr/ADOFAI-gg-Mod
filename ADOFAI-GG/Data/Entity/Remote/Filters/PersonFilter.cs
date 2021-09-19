using ADOFAI_GG.Data.Entity.Remote.SortOrder;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public sealed class PersonFilter : SearchFilter {
        public PersonFilter(int offset, int amount) : base(offset, amount) { }
        public string name {
            get => (string) Filters["name"];
            set => Filters["name"] = value;
        }
        
        public PersonsSortOrder? sort {
            get => (PersonsSortOrder?) Filters["sort"];
            set => Filters["sort"] = value;
        }

        public double? minTotalPp {
            get => (double?) Filters["minTotalPp"];
            set => Filters["minTotalPp"] = value;
        }

        public double? maxTotalPp {
            get => (double?) Filters["maxTotalPp"];
            set => Filters["maxTotalPp"] = value;
        }
    }
}