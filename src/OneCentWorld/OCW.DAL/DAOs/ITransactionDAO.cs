using System;
using System.Collections.Generic;
using OCW.DAL.DTOs;

namespace OCW.DAL.DAOs
{
    public interface ITransactionDAO : IDAO<Transaction, int, IEnumerable<Transaction>>
    {
        IEnumerable<Transaction> ReadAllFull(Func<Transaction, bool> predicate = null);
    }
}
