using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;

namespace screen_time_tracker_react_app.People
{
    public class CsvPersonRepository : IPersonRepository
    {
        private const string FilePath = "People.csv";

        public Task<Person> AddPerson(Person person)
        {
            int id = 0;
            if (!File.Exists(FilePath)) 
            {
                File.WriteAllLines(FilePath, new List<string> { $"1,Evalan Naidu,{new DateTime(1988, 09, 02)}" });
                id = 1;
            }
            else
            {
                id = GetNextId();
            }

            var newPerson = new Person(id, person.Name, person.DateOfBirth);

            File.AppendAllLines(FilePath, new string[1] { $"{newPerson.Id},{newPerson.Name},{newPerson.DateOfBirth}" });

            return Task.FromResult(newPerson);
        }

        public Task<IEnumerable<Person>> GetPeople()
        {
            if (!File.Exists(FilePath)) File.WriteAllLines(FilePath, new List<string> { $"1,Evalan Naidu,{new DateTime(1988, 09, 02)}" });
            var csv = File.ReadAllLines(FilePath);
            var people = new List<Person>(csv.Length);

            foreach (var line in csv)
            {
                var (id, name, dob) = ParseLine(line);
                people.Add(new Person(id, name, dob));
            }

            return Task.FromResult<IEnumerable<Person>>(people);
        }

        private (int id, string name, DateTime dob) ParseLine(string line)
        {
            var split = line.Split(',');
            var id = int.Parse(split[0]);
            var dob = DateTime.Parse(split[2]);
            return (id, split[1], dob);
        }

        private int GetNextId()
        {
            var id = 1;
            if (File.Exists(FilePath))
            {
                var last = File.ReadLines(FilePath).Last();
                var (lastId, _, __) = ParseLine(last);
                id = ++lastId;
            }

            return id;
        }
    }
}