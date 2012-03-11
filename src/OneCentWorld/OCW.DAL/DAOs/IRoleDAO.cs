using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IRoleDAO : IDAO<Role, int, IEnumerable<Role>>
    {
        Role FindByName(string name);
    }
}
