using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IAccountDAO : IDAO<Account, int, IEnumerable<Account>>
    {
    }
}
