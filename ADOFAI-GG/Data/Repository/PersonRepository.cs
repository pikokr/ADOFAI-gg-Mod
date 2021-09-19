using ADOFAI_GG.Data.Entity.Remote.Filters;
using ADOFAI_GG.Data.Entity.Remote.Types;
using ADOFAI_GG.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using TinyJSON.Types;

namespace ADOFAI_GG.Data.Repository
{
    class PersonRepository
    {
        private static PersonRepository instance;

        public static PersonRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new PersonRepository();
            }
            return instance;
        }

        protected PersonRepository()
        {
        }

        public async Task<(List<Person>, int)?> RequestPeople(PersonFilter filter)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/person?{filter}");
            if (json == null) return null;
            var result = new List<Person>();
            var results = json["results"];

            foreach (var person in (ProxyArray)results)
            {
                result.Add(Person.FromJson(person));
            }

            return (result, json["count"]);
        }

        public async Task<Person> RequestPerson(int id)
        {
            var json = await NetworkUtil.GetJsonAsync($"api/v1/person/{id}");
            if (json == null) return null;
            return Person.FromJson(json);
        }


    }
}
