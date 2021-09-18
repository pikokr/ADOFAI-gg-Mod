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
            foreach ((string key, object value) in  filters) query.Add(key, $"{value}");
            return query.ToString();
        }

    }

    public static class SearchFilterExtension
    {

        public static T Offset<T>(this T filter, int value) where T : SearchFilter
        {
            filter.offset = value;
            return filter;
        }

        public static T Amount<T>(this T filter, int value) where T : SearchFilter
        {
            filter.amount = value;
            return filter;
        }

    }

}