using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using screen_time_tracker_react_app.People;

namespace screen_time_tracker_api_tests
{
    [TestFixture]
    public class CsvPersonRepositoryTests
    {
        private string _filePath;

        [SetUp]
        public void SetUp()
        {
            _filePath = "PeopleFile.csv";
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
            _filePath = "NonExistentFile.csv";
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
            Assert.That(people[0].Name, Is.EqualTo("Evalan Naidu"));
            Assert.That(people[1].Name, Is.EqualTo("Wade Wilson"));
            Assert.That(people[2].Name, Is.EqualTo("Logan"));
            Assert.That(people[3].Name, Is.EqualTo("Peter Parker"));
        }

        private CsvPersonRepository CreateSut()
        {
            return new CsvPersonRepository(_filePath);
        }
    }
}