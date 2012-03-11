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
    public class CompanyDAO : ICompanyDAO, ICompanyFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public CompanyDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region ICompanyDAO Implementation
        public Company Add(Company entity)
        {
            if (entity.Id > 0) return entity;
            context.Profile.AddObject(entity);

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

        public Company Read(int key)
        {
            var listCompany = context.Profile.OfType<Company>().Where(c => key == c.Id);
            if(listCompany.Count() == 0) throw new RecordNotFoundException<int?>("Company", key);
            Company company = listCompany.First();

            return company;
        }

        public IEnumerable<Company> ReadAll(Func<Company, bool> predicate = null)
        {
            return context.Profile.OfType<Company>().Where(predicate ?? (p => true));
        }

        public IEnumerable<Company> ReadAll(int pageSize, int page, Func<Company, bool> predicate)
        {
            return context.Profile.OfType<Company>().Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Company, bool> predicate)
        {
            return context.Profile.OfType<Company>().Where(predicate ?? (p => true)).Count();
        }

        public Company Update(Company entity)
        {
            return entity;
        }

        public void Delete(Company entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var companyList = context.Profile.OfType<Company>().Where(c => c.Id == entity);
            if (companyList.Count() == 0) throw new RecordNotFoundException<int>("Company", entity);
            var company = companyList.First();

            context.DeleteObject(company);
        }

        #endregion

        #region ICompanyFactory Implementation
        public Company Create(int id, string email, string password, DateTime registeredDate, int addressId, int accountId, string name, byte[] image, string url, IEnumerable<int> tagIds)
        {
            Company c = context.CreateObject<Company>();
            c.Id = id;
            c.Email = email;
            c.Password = password;
            c.RegisteredDate = registeredDate;
            c.Address_id = addressId;
            c.Account_id = accountId;
            c.Name = name;
            c.Image = image;
            c.Url = url;

            foreach (int tagId in tagIds)
            {
                var tag = context.Tag.OfType<Tag>().Where(t => t.Id == tagId);
                c.Tag.Add(tag.First());
            }

            return c;
        }
        #endregion
    }
}
