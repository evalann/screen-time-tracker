using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace screen_time_tracker_react_app.People
{
    public class CsvPersonRepository : IPersonRepository
    {
        private const string FilePath = "People.csv";

        public Task AddPerson(Person person)
        {
            File.AppendAllLines(FilePath, new string [1] {person.Name});
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Person>> GetPeople()
        {
            var csv = File.ReadAllLines(FilePath);
            var people = new List<Person>(csv.Length);

            foreach(var line in csv)
            {
                people.Add(new Person(line));
            }

            return Task.FromResult<IEnumerable<Person>>(people);
        }
    }
}