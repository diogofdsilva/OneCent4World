using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IProfileDAO : IDAO<Profile, int, IEnumerable<Profile>>
    {
        Profile FindByEmail(string email);
    }
}
