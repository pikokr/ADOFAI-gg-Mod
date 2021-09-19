using ADOFAI_GG.Utils;
using System.Globalization;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Entity.Remote.Types {
	public class IdNameDto {
		public readonly int Id;
		public readonly string Name;

		public IdNameDto(int id, string name) {
			Id = id;
			Name = name;
		}

		public static IdNameDto FromJson(Variant obj) {
			return new IdNameDto(
				obj.GetOrNull("id")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("name")?.ToString(CultureInfo.InvariantCulture) ?? ""
			);
		}
	}
}