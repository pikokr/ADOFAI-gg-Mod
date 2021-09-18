using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.Utilities;

namespace ADOFAI_GG.API.Types {
    public enum TagType {
        Invalid,
        Level
    }
    public readonly struct Tag {
        public readonly int id;
        public readonly TagType tagType;
        public readonly string name;
        private static Dictionary<TagType, Dictionary<int, string>> _tags;

        [Init] public static void LoadTags() => _tags = Request.RequestTags().Result;
        
        public Tag(TagType tagType, int id) {
            this.tagType = tagType;
            this.id = id;
            this.name = _tags[tagType][id];
        }
                
        public Tag(TagType tagType, string name) {
            this.tagType = tagType;
            this.name = name;
            this.id = _tags[tagType].First(pair => pair.Value == name).Key;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Tag)) return false;
            var tag = (Tag) obj;
            return tag.tagType == tagType && tag.id == id;
        }

        public bool Equals(Tag other) {
            return id == other.id && tagType == other.tagType && name == other.name;
        }

        public override int GetHashCode() {
            unchecked {
                int hashCode = id;
                hashCode = (hashCode * 397) ^ (int) tagType;
                hashCode = (hashCode * 397) ^ (name != null ? name.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Tag a, Tag b) => a.Equals(b);
        public static bool operator !=(Tag a, Tag b) => !(a.Equals(b));
        public override string ToString() => name;
    }
}