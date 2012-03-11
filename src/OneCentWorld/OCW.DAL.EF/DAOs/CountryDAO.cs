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
    public class CountryDAO : ICountryDAO, ICountryFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public CountryDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region ICountryDAO Implementation
        public Country Add(Country entity)
        {
            if (entity.Id > 0) return entity;

            context.Country.AddObject(entity);

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

        public Country Read(int key)
        {
            var listCountry = context.Country.Where(c => key == c.Id);
            if(listCountry.Count() == 0) throw new RecordNotFoundException<int?>("Country", key);
            Country country = listCountry.First();

            return country;
        }

        public IEnumerable<Country> ReadAll(Func<Country, bool> predicate = null)
        {
            return context.Country.Where(predicate ?? (p => true));
        }

        public IEnumerable<Country> ReadAll(int pageSize, int page, Func<Country, bool> predicate)
        {
            return context.Country.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Country, bool> predicate)
        {
            return context.Country.Where(predicate ?? (p => true)).Count();
        }

        public Country Update(Country entity)
        {
            return entity;
        }

        public void Delete(Country entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var countryList = context.Country.Where(c => c.Id == entity);
            if (countryList.Count() == 0) throw new RecordNotFoundException<int>("Country", entity);
            var country = countryList.First();

            context.DeleteObject(country);
        }

        #endregion

        #region ICountryFactory Implementation
        public Country Create(int id, string name, string alpha2, string alpha3, string numeric)
        {
            Country c = context.CreateObject<Country>();
            c.Id = id;
            c.Name = name;
            c.Alpha2 = alpha2;
            c.Alpha3 = alpha3;
            c.Numeric = numeric;
            return c;
        }
        #endregion
    }
}
