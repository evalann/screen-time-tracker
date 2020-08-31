using System.Collections.Generic;
using System.Threading.Tasks;

namespace screen_time_tracker_react_app.People
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> GetPeople();
        Task AddPerson(Person person);
    }
}