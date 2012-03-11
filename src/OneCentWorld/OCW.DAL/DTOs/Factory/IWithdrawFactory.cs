using System;

namespace OCW.DAL.DTOs.Factory
{
    public interface IWithdrawFactory
    {
        Withdraw Create(int id, DateTime date, decimal amount, string destination, int profileId);
    }
}
