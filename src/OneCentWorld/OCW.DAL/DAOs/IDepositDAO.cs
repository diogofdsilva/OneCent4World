using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IDepositDAO : IDAO<Deposit, int, IEnumerable<Deposit>>
    {
    }
}