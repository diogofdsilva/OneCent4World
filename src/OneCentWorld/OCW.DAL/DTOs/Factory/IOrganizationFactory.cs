using System;
using System.Collections.Generic;

namespace OCW.DAL.DTOs.Factory
{
    public interface IOrganizationFactory
    {
        Organization Create(int id, string email, string password, DateTime registeredDate, int addressId, int accountId, string name, byte[] image, string url,IEnumerable<int> tagIds);
    }
}
