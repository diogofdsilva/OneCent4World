using System;

namespace OCW.DAL.DTOs.Factory
{
    public interface ITransactionFactory
    {
        Transaction Create(int id, decimal value, decimal donation, DateTime date, string reference, int personId, int companyId,
                           int? organizationId);
    }
}
