using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs;
using OCW.DAL.DTOs.Factory;
using OCW.DAL.Exceptions;

namespace OCW.DAL.EF.DAOs
{
    public class EmergencyDAO : IEmergencyDAO, IEmergencyFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public EmergencyDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IEmergencyDAO Implementation
        public Emergency Add(Emergency entity)
        {
            if (entity.Id > 0) return entity;

            context.Emergency.AddObject(entity);

            try
            {
                context.SaveChanges();
            }
            catch (OptimisticConcurrencyException e)
            {
                //An optimistic concurrency violation has occurred in the data source.
                throw new ConcurrencyException(e.Message);
            }

            return entity;
        }

        public Emergency Read(int key)
        {
            var listEmergency = context.Emergency.Where(e => key == e.Id);
            if (listEmergency.Count() == 0) throw new RecordNotFoundException<int?>("Emergency", key);
            Emergency emergency = listEmergency.First();

            return emergency;
        }

        public IEnumerable<Emergency> ReadAll(Func<Emergency, bool> predicate = null)
        {
            return context.Emergency.Where(predicate ?? (p => true));
        }

        public IEnumerable<Emergency> ReadAll(int pageSize, int page, Func<Emergency, bool> predicate)
        {
            return context.Emergency.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Emergency, bool> predicate)
        {
            return context.Emergency.Where(predicate ?? (p => true)).Count();
        }

        public Emergency Update(Emergency entity)
        {
            return entity;
        }

        public void Delete(Emergency entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var emergencyList = context.Emergency.Where(e => e.Id == entity);
            if (emergencyList.Count() == 0) throw new RecordNotFoundException<int>("Emergency", entity);
            var emergency = emergencyList.First();

            context.DeleteObject(emergency);
        }

        #endregion

        #region IEmergencyFactory Implementation
        public Emergency Create(int id, string title, string description, DateTime startDate, DateTime? endDate, decimal weight, byte[] image, int organizationId)
        {
            Emergency e = context.CreateObject<Emergency>();
            e.Id = id;
            e.Title = title;
            e.Description = description;
            e.StartDate = startDate;
            e.EndDate = endDate;
            e.Weight = weight;
            e.Image = image;
            e.Organization_id = organizationId;
            return e;
        }
        #endregion
    }
}