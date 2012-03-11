using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface ICompanyDAO : IDAO<Company, int, IEnumerable<Company>>
    {
    }
}
