using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface IWithdrawDAO : IDAO<Withdraw, int, IEnumerable<Withdraw>>
    {
    }
}