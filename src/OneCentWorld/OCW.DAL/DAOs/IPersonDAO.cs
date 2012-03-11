using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IPersonDAO : IDAO<Person, int, IEnumerable<Person>>
    {
    }
}
