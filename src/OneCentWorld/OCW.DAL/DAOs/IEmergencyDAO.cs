using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IEmergencyDAO : IDAO<Emergency, int, IEnumerable<Emergency>>
    {
    }
}
