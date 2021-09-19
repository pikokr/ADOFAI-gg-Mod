using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository {
    class PersonRepository {
        private static PersonRepository _instance;
        public static PersonRepository Instance => _instance ??= new PersonRepository();
        protected PersonRepository() { }

        public async Task<(List<Person>, int)?> RequestPeople(PersonFilter filter) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/person?{filter}");
            if (json == null) return null;
            var results = json["results"];

            var result = ((ProxyArray) results).Select(Person.FromJson).ToList();

            return (result, json["count"]);
        }

        public async Task<Person> RequestPerson(int id) {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/person/{id}");
            return json == null ? null : Person.FromJson(json);
        }
    }
}