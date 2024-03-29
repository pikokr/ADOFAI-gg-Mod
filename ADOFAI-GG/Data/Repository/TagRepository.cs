﻿using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository {
    class TagRepository {
        private static TagRepository _instance;
        public static TagRepository Instance => _instance ??= new TagRepository();
        protected TagRepository() { }

        public async Task<Dictionary<TagType, Dictionary<int, string>>> RequestTags() {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/tags");
            if (json == null) return null;
            var results = json["results"];
            var allTags = new List<(int, TagType, string)>();

            foreach (var tag in (ProxyArray) results) {
                int id = tag["id"].ToInt32(CultureInfo.InvariantCulture);
                string typeName = tag["type"].ToString(CultureInfo.InvariantCulture);
                string name = tag["name"].ToString(CultureInfo.InvariantCulture);

                if (Enum.TryParse<TagType>(typeName, out var tagType)) {
                    allTags.Add((id, tagType, name));
                }
            }

            var result = new Dictionary<TagType, Dictionary<int, string>>();
            foreach (int tag in Enum.GetValues(typeof(TagType))) {
                var tags = new Dictionary<int, string>();
                foreach ((int id, _, string name) in allTags.Where(tuple => tuple.Item2 == (TagType) tag)) {
                    tags[id] = name;
                }
                result.Add((TagType) tag, tags);
            }
            return result;
        }
    }
}