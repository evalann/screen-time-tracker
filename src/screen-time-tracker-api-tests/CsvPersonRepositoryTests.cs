using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using screen_time_tracker_react_app.People;

namespace screen_time_tracker_api_tests
{
    [Explicit("These are integration tests that will write to a csv file.")]
    [TestFixture]
    public class CsvPersonRepositoryTests
    {
        private const string FilePath = "People.csv";
        private string[] _defaultContents = new string[] {$"Evalan Naidu,{new DateTime(1988, 09, 02)}", $"Wade Wilson,{new DateTime(1950, 05, 06)}", $"Logan,{new DateTime(1900, 01, 03)}", $"Peter Parker,{new DateTime(1988, 06, 13)}"};
        
        [SetUp]
        public void SetUp()
        {
            File.WriteAllLines(FilePath, _defaultContents);
        }

        [Test]
        public void CsvPersonRepository_GivenNewInstance_ShouldImplementIPersonRepository()
        {
            var sut = CreateSut();

            Assert.That(sut, Is.InstanceOf<IPersonRepository>());
        }

        [Test]
        public async Task GetPeople_GivenNoFile_ShouldCreateFileWithDefaultPerson()
        {
            DeleteFileIfExists();
            var sut = CreateSut();

            var act = await sut.GetPeople();

            Assert.That(act.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetPeople_GivenFileWithMultiplePeople_ShouldReturnPeople()
        {
            var sut = CreateSut();

            var act = await sut.GetPeople();

            Assert.That(act, Is.Not.Null);
            var people = act.ToList();
            AssertDefaultPeopleAreIn(people);
        }

        [Test]
        public async Task AddPerson_GivenCsvFileDoesNotExist_ShouldCreateAndAddPerson()
        {
            var dob = new DateTime(1988, 09, 02);
            const string name = "Evalan Naidu";
            var expected = $"{name},{dob}";
            DeleteFileIfExists();
            var sut = CreateSut();

            await sut.AddPerson(new Person(name, dob));

            var lines = File.ReadAllLines(FilePath);
            Assert.That(lines, Contains.Item(expected));
        }

        [Test]
        public async Task AddPerson_GivenFileAlreadyHasContents_ShouldAppendNewPersonToEnd()
        {
            var dob = new DateTime(1989, 02, 01);
            const string name = "Mary Jane Watson";
            var expected = $"{name},{dob}";
            var fileContentsBefore = File.ReadAllLines(FilePath);
            Assert.That(fileContentsBefore, Does.Not.Contain(expected));
            var sut = CreateSut();

            await sut.AddPerson(new Person(name, dob));

            var fileContentsAfter = File.ReadAllLines(FilePath);
            Assert.That(fileContentsAfter, Contains.Item(expected));
            AssertDefaultPeopleAreIn(fileContentsAfter);
        }

        private CsvPersonRepository CreateSut()
        {
            return new CsvPersonRepository();
        }

        private void AssertDefaultPeopleAreIn(List<Person> people)
        {
            for (int i = 0; i < _defaultContents.Length; i++)
            {
                var split = _defaultContents[i].Split(',');
                Assert.That(people[i].Name, Is.EqualTo(split[0]));
                Assert.That(people[i].DateOfBirth.ToString(), Is.EqualTo(split[1]));
            }
        }

        private void AssertDefaultPeopleAreIn(string[] currentFileContents)
        {
            for (int i = 0; i < _defaultContents.Length; i++)
            {
                Assert.That(currentFileContents[i], Is.EqualTo(_defaultContents[i]));
            }
        }

        private static void DeleteFileIfExists()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
    }
}