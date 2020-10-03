using System;

namespace screen_time_tracker_react_app.People
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Person() { }

        public Person(int? id, string name, DateTime dateOfBirth)
        {
            Id = id ?? 0;
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}