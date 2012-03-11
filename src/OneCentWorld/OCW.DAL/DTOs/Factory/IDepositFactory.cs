using System;

namespace OCW.DAL.DTOs.Factory
{
    public interface IDepositFactory
    {
        Deposit Create(int id, DateTime date, decimal amount, string source, int profileId);
    }
}
