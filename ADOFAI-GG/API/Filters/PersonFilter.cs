using ADOFAI_GG.API.SortOrder;

namespace ADOFAI_GG.API.Filters {
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

        private double? minTotalPP {
            get => (double?) filters["minTotalPp"];
            set => filters["minTotalPp"] = value;
        }

        private double? maxTotalPP {
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

        public PersonFilter MinTotalPP(double value) {
            minTotalPP = value;
            return this;
        }

        public PersonFilter MaxTotalPP(double value) {
            maxTotalPP = value;
            return this;
        }

        public PersonFilter Custom(string key, object value) {
            filters[key] = value;
            return this;
        }
    }
}