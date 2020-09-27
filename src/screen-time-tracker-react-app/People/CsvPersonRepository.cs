using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;

namespace screen_time_tracker_react_app.People
{
    public class CsvPersonRepository : IPersonRepository
    {
        private const string FilePath = "People.csv";

        public Task AddPerson(Person person)
        {
            File.AppendAllLines(FilePath, new string [1] {$"{person.Name},{person.DateOfBirth}"});
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Person>> GetPeople()
        {
            var csv = File.ReadAllLines(FilePath);
            var people = new List<Person>(csv.Length);

            foreach(var line in csv)
            {
                var split = line.Split(',');
                var dob = DateTime.Parse(split[1]);
                people.Add(new Person(split[0], dob));
            }

            return Task.FromResult<IEnumerable<Person>>(people);
        }
    }
}