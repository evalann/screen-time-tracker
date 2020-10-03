using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using screen_time_tracker_react_app.Controllers;
using System.Threading.Tasks;
using System.Collections.Generic;
using screen_time_tracker_react_app.People;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;

namespace screen_time_tracker_api_tests
{
    [TestFixture]
    public class PeopleControllerTests
    {
        private IPersonRepository _personRepository;

        [SetUp]
        public void SetUp()
        {
            _personRepository = Substitute.For<IPersonRepository>();
        }

        [Test]
        public void PeopleController_GivenNewInstance_ShouldInheritFromControllerBase()
        {
            var sut = CreateSut();

            Assert.That(sut, Is.InstanceOf<ControllerBase>());
        }

        [Test]
        public async Task GetPeople_GivenNoPeople_ShouldReturnOkWithNoResult()
        {
            _personRepository.GetPeople().Returns((IEnumerable<Person>)null);
            var sut = CreateSut();

            var act = await sut.GetPeople();

            Assert.That(act, Is.InstanceOf<OkObjectResult>());
            var response = (OkObjectResult)act;
            Assert.That(response.Value, Is.Null);
        }

        [Test]
        public async Task GetPeople_GivenPersonRepositoryReturnsPeople_ShouldReturnOkWithBodyPopulated()
        {
            var expected = new List<Person> {new Person(1, "Evalan Naidu", new DateTime(1988, 09, 02)), new Person(2, "Wade Wilson", new DateTime(1950, 08, 16))};
            _personRepository.GetPeople().Returns(expected);
            var sut = CreateSut();
            
            var act = (OkObjectResult)await sut.GetPeople();

            Assert.That(act.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetPeople_GivenExceptionThrownInPersonRepository_ShouldRethrow()
        {
            var expected = new Exception("Something went wrong.");
            _personRepository.GetPeople().Throws(expected);
            var sut = CreateSut();

            var act = Assert.ThrowsAsync<Exception>(() => sut.GetPeople());

            Assert.That(act, Is.EqualTo(expected));
        }

        [Test]
        public async Task AddPerson_GivenPerson_ShouldAddAndReturnOkWithPersonInResponse()
        {
            var person = new Person(null, "Deadpool", new DateTime(1950, 12, 01));
            var expected = new Person(1, person.Name, person.DateOfBirth);
            _personRepository.AddPerson(person).Returns(expected);
            var sut = CreateSut();

            var act = await sut.AddPerson(person);

            Assert.That(act, Is.InstanceOf<OkObjectResult>());
            var actualPerson = ((OkObjectResult)act).Value;
            Assert.That(actualPerson, Is.EqualTo(expected));
        }

        [Test]
        public async Task AddPerson_GivenNull_ShouldReturnBadRequest()
        {
            var sut = CreateSut();

            var act = await sut.AddPerson(null);

            Assert.That(act, Is.InstanceOf<BadRequestObjectResult>());
            var response = (BadRequestObjectResult)act;
            Assert.That(response.Value, Is.EqualTo("Person was not provided."));
        }

        private PeopleController CreateSut()
        {
            return new PeopleController(_personRepository);
        }
    }
}