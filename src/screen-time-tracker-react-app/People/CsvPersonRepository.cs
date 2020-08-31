using System.Collections.Generic;
using System.Threading.Tasks;

namespace screen_time_tracker_react_app.People
{
    public class CsvPersonRepository : IPersonRepository
    {
        private string _filePath;

        public CsvPersonRepository(string filePath)
        {
            _filePath = filePath;
        }

        public Task AddPerson(Person person)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Person>> GetPeople()
        {
            var csv = System.IO.File.ReadAllLines(_filePath);
            var people = new List<Person>(csv.Length);

            foreach(var line in csv)
            {
                people.Add(new Person(line));
            }

            return Task.FromResult<IEnumerable<Person>>(people);
        }
    }
}