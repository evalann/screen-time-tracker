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
        private string[] _defaultContents = new string[] {"Evalan Naidu", "Wade Wilson", "Logan", "Peter Parker"};

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
        public void GetPeople_GivenNoFile_ShouldThrowFileNotFoundException()
        {
            DeleteFileIfExists();
            var sut = CreateSut();

            Assert.ThrowsAsync<FileNotFoundException>(() => sut.GetPeople());
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
            const string expected = "Evalan Naidu";
            DeleteFileIfExists();
            var sut = CreateSut();

            await sut.AddPerson(new Person(expected));

            var lines = File.ReadAllLines(FilePath);
            Assert.That(lines, Contains.Item(expected));
        }

        [Test]
        public async Task AddPerson_GivenFileAlreadyHasContents_ShouldAppendNewPersonToEnd()
        {
            var expected = "Mary Jane Watson";
            var fileContentsBefore = File.ReadAllLines(FilePath);
            Assert.That(fileContentsBefore, Does.Not.Contain(expected));
            var sut = CreateSut();

            await sut.AddPerson(new Person(expected));

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
                Assert.That(people[i].Name, Is.EqualTo(_defaultContents[i]));
            }
        }

        private void AssertDefaultPeopleAreIn(string[] people)
        {
            for (int i = 0; i < _defaultContents.Length; i++)
            {
                Assert.That(people[i], Is.EqualTo(_defaultContents[i]));
            }
        }

        private static void DeleteFileIfExists()
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
    }
}