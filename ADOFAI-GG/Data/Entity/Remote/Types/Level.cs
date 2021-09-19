using System.Globalization;
using TinyJSON.Types;
using ADOFAI_GG.Utils;
using System.Linq;

namespace ADOFAI_GG.Data.Entity.Remote.Types {
	public class Level {
		public readonly int Id;
		public readonly string Title;
		public readonly double Difficulty;
		public readonly string Description;
		public readonly string Video;
		public readonly string Download;
		public readonly string Workshop;
		public readonly bool EpilepsyWarning;
		public readonly string[] Creators;
		public readonly int SongId;
		public readonly string Song;
		public readonly string[] Artists;
		public readonly double MinBPM;
		public readonly double MaxBPM;
		public readonly int Tiles;
		public readonly int Comments;
		public readonly int Likes;
		public readonly IdNameDto[] Tags;

		public Level(int id, string title, double difficulty, string description, string video, string download,
			string workshop, bool epilepsyWarning, string[] creators, int songId, string song,
			string[] artists, double minBpm, double maxBpm, int tiles, int comments, int likes, IdNameDto[] tags) {
			Id = id;
			Title = title;
			Difficulty = difficulty;
			Description = description;
			Video = video;
			Download = download;
			Workshop = workshop;
			EpilepsyWarning = epilepsyWarning;
			Creators = creators;
			SongId = songId;
			Song = song;
			Artists = artists;
			MinBPM = minBpm;
			MaxBPM = maxBpm;
			Tiles = tiles;
			Comments = comments;
			Likes = likes;
			Tags = tags;
		}

		public static Level FromJson(Variant obj) {
			return new Level(
				obj.GetOrNull("id")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("title")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("difficulty")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("description")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("video")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("download")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("workshop")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("epilepsyWarning")?.ToBoolean(CultureInfo.InvariantCulture) ?? false,
				obj.GetOrNull("creators")?.Make<string[]>(),
				obj.GetOrNull("songId")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("song")?.ToString(CultureInfo.InvariantCulture),
				obj.GetOrNull("artists")?.Make<string[]>(),
				obj.GetOrNull("minBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("maxBpm")?.ToDouble(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("tiles")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("comments")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				obj.GetOrNull("likes")?.ToInt32(CultureInfo.InvariantCulture) ?? -1,
				(from tag in (ProxyArray)obj["tags"] select IdNameDto.FromJson(tag)).ToArray()
			);
		}

	}
}