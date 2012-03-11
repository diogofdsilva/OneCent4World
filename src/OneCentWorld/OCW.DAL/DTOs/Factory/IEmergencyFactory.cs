using System;

namespace OCW.DAL.DTOs.Factory
{
    public interface IEmergencyFactory
    {
        Emergency Create(int id, string title, string description, DateTime startDate, DateTime? endDate, decimal weight,
                         byte[] image, int organizationId);
    }
}
