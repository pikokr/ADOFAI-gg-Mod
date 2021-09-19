using ADOFAI_GG.Data.Entity.Remote.SortOrder;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public sealed class PersonFilter : SearchFilter {
        public PersonFilter(int offset, int amount) : base(offset, amount) { }
        private string name {
            get => (string) filters["name"];
            set => filters["name"] = value;
        }
        
        private PersonsSortOrder? sort {
            get => (PersonsSortOrder?) filters["sort"];
            set => filters["sort"] = value;
        }

        private double? minTotalPp {
            get => (double?) filters["minTotalPp"];
            set => filters["minTotalPp"] = value;
        }

        private double? maxTotalPp {
            get => (double?) filters["maxTotalPp"];
            set => filters["maxTotalPp"] = value;
        }

        public PersonFilter Name(string value) {
            name = value;
            return this;
        }
        
        public PersonFilter Sort(PersonsSortOrder value) {
            sort = value;
            return this;
        }

        public PersonFilter MinTotalPp(double value) {
            minTotalPp = value;
            return this;
        }

        public PersonFilter MaxTotalPp(double value) {
            maxTotalPp = value;
            return this;
        }

        public PersonFilter Custom(string key, object value) {
            filters[key] = value;
            return this;
        }
    }
}