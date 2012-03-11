using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IAddressDAO : IDAO<Address, int, IEnumerable<Address>>
    {
    }
}
