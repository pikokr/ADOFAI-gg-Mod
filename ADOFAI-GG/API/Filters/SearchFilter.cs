using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADOFAI_GG.API.Filters {
    public abstract class SearchFilter {
        protected SearchFilter(int offset, int amount) {
            this.offset = offset;
            this.amount = amount;
        }
        
        protected Dictionary<string, object> filters = new();
        internal int offset {
            get => (int) filters["offset"];
            set => filters["offset"] = value;
        }

        internal int amount {
            get => (int) filters["amount"];
            set => filters["amount"] = value;
        }


        public override string ToString() {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach ((string key, object value) in filters) query.Add(key, $"{value}");
            return query.ToString();
        }
    }
}