using System;

namespace screen_time_tracker_react_app.People
{
    public class Person
    {
        public string Name { get; }
        public DateTime DateOfBirth { get; }

        public Person(string name, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
        }
    }
}