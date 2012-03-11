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
    public class OrganizationDAO : IOrganizationDAO, IOrganizationFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public OrganizationDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IOrganizationDAO Implementation
        public Organization Add(Organization entity)
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

        public Organization Read(int key)
        {
            var listOrganization = context.Profile.OfType<Organization>().Where(o => key == o.Id);
            if(listOrganization.Count() == 0) throw new RecordNotFoundException<int?>("Organization", key);
            Organization organization = listOrganization.First();

            return organization;
        }

        public IEnumerable<Organization> ReadAll(Func<Organization, bool> predicate = null)
        {
            return context.Profile.OfType<Organization>().Where(predicate ?? (p=>true));
        }

        public IEnumerable<Organization> ReadAll(int pageSize, int page, Func<Organization, bool> predicate)
        {
            return context.Profile.OfType<Organization>().Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Organization, bool> predicate)
        {
            return context.Profile.OfType<Organization>().Where(predicate ?? (p => true)).Count();
        }

        public Organization Update(Organization entity)
        {
            return entity;
        }

        public void Delete(Organization entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var organizationList = context.Profile.OfType<Organization>().Where(o => o.Id == entity);
            if (organizationList.Count() == 0) throw new RecordNotFoundException<int>("Organization", entity);
            var organization = organizationList.First();

            context.DeleteObject(organization);
        }

        #endregion

        #region IOrganizationFactory Implementation
        public Organization Create(int id, string email, string password, DateTime registeredDate, int addressId, int accountId, string name, byte[] image, string url, IEnumerable<int> tagIds)
        {
            Organization o = context.CreateObject<Organization>();
            o.Id = id;
            o.Email = email;
            o.Password = password;
            o.RegisteredDate = registeredDate;
            o.Address_id = addressId;
            o.Account_id = accountId;
            o.Name = name;
            o.Image = image;
            o.Url = url;

            foreach (int tagId in tagIds)
            {
                var tag = context.Tag.OfType<Tag>().Where(t => t.Id == tagId);
                //if (listOrganization.Count() == 0) throw new RecordNotFoundException<int?>("Organization", key);
                o.Tag.Add(tag.First());
            }

            return o;
        }
        #endregion
    }
}
