using System.Globalization;
using TinyJSON.Types;
using ADOFAI_GG.Utils;

namespace ADOFAI_GG.Data.Entity.Remote.Types {
    public class Ranking : Person {
        public readonly SimplePlayLogWithLevel BestPlay;

        internal Ranking(int id, string name, double totalBpm, double totalPp, SimplePlayLogWithLevel bestPlay) : base(
            id, name, totalBpm, totalPp) {
            BestPlay = bestPlay;
        }

        public new static Ranking FromJson(Variant obj) {
            return new Ranking(
                obj.GetOrNull("id").ToInt32(CultureInfo.InvariantCulture),
                obj.GetOrNull("name")?.ToString(CultureInfo.InvariantCulture),
                obj.GetOrNull("totalBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                obj.GetOrNull("totalPp")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
                SimplePlayLogWithLevel.FromJson(obj["bestPlay"])
            );
        }
    }
}