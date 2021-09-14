using ADOFAI_GG.API.Filters;

namespace ADOFAI_GG.API {
    public static class Extensions {
        public static T Offset<T>(this T filter, int value) where T : SearchFilter {
            filter.offset = value;
            return filter;
        }
        
        public static T Amount<T>(this T filter, int value) where T : SearchFilter {
            filter.amount = value;
            return filter;
        }
    }
}