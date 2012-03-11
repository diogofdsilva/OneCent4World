using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IOrganizationDAO : IDAO<Organization, int, IEnumerable<Organization>>
    {
    }
}
