using System;

namespace OCW.DAL.DTOs.Factory
{
    public interface IPersonFactory
    {
        Person Create(int id, string email, string password, DateTime registeredDate, int addressId, int accountId,
                      string firstName, string lastName, DateTime? birthDate, int roleId);
    }
}
