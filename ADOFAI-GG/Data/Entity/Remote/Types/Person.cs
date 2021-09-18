using System.Globalization;
using TinyJSON.Types;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data.Entity.Remote.Types {
    public class Person {
        public readonly int Id;
        public readonly string Name;
        public readonly double TotalBPM;
        public readonly double TotalPP;

        internal Person(int id, string name, double totalBpm, double totalPp) {
            Id = id;
            Name = name;
            TotalBPM = totalBpm;
            TotalPP = totalPp;
        }

        public static Person FromJson(Variant obj) {
            return new Person(
                obj.GetOrNull("id")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("name")?.ToString(CultureInfo.InvariantCulture),
                obj.GetOrNull("totalBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("totalPp")?.ToDouble(CultureInfo.InvariantCulture) ?? -1
            );
        }
    }
}