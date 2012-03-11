using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface ICountryDAO : IDAO<Country, int, IEnumerable<Country>>
    {
    }
}
