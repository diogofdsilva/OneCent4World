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
    public class AddressDAO : IAddressDAO, IAddressFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public AddressDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IAddressDAO Implementation
        public Address Add(Address entity)
        {
            if (entity.Id > 0) return entity;

            context.Address.AddObject(entity);

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

        public Address Read(int key)
        {
            var listAddress = context.Address.Where(a => key == a.Id);
            if (listAddress.Count() == 0) throw new RecordNotFoundException<int?>("Address", key);
            Address address = listAddress.First();

            return address;
        }

        public IEnumerable<Address> ReadAll(Func<Address, bool> predicate = null)
        {
            return context.Address.Where(predicate ?? (p => true));
        }

        public IEnumerable<Address> ReadAll(int pageSize, int page, Func<Address, bool> predicate)
        {
            return context.Address.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Address, bool> predicate)
        {
            return context.Address.Where(predicate ?? (p => true)).Count();
        }

        public Address Update(Address entity)
        {
            return entity;
        }

        public void Delete(Address entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var addressList = context.Address.Where(a => a.Id == entity);
            if (addressList.Count() == 0) throw new RecordNotFoundException<int>("Address", entity);
            var address = addressList.First();

            context.DeleteObject(address);
        }

        #endregion

        #region IAddressFactory Implementation
        public Address Create(int id, string address1, string address2, string postalCode, string city, int countryId)
        {
            Address a = context.CreateObject<Address>();
            a.Id = id;
            a.Address1 = address1;
            a.Address2 = address2;
            a.PostalCode = postalCode;
            a.City = city;
            a.Country_id = countryId;

            return a;
        }
        #endregion
    }
}
