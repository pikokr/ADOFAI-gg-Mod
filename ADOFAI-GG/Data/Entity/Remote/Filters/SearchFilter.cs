using System;
using System.Collections.Generic;
using System.Web;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data.Entity.Remote.Filters {
    public abstract class SearchFilter {
        protected SearchFilter(int offset, int amount) {
            this.offset = offset;
            this.amount = amount;
        }
        protected readonly Dictionary<string, object> Filters = new();
        public int offset {
            get => (int) Filters["offset"];
            set => Filters["offset"] = value;
        }

        public int amount {
            get => (int) Filters["amount"];
            set => Filters["amount"] = value;
        }

        public override string ToString() {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, object value) in  Filters) query.Add(key, $"{value}");
            return query.ToString();
        }
    }
}