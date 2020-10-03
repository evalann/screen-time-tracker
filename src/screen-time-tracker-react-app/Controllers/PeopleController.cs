using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using screen_time_tracker_react_app.People;

namespace screen_time_tracker_react_app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private IPersonRepository _personRepository;

        public PeopleController(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            return Ok(await _personRepository.GetPeople());
        }

        [HttpPost]
        public async Task<IActionResult> AddPerson(Person person)
        {
            if(person == null) return BadRequest("Person was not provided.");
            
            var result = await _personRepository.AddPerson(person);
            return Ok(result);
        }
    }
}