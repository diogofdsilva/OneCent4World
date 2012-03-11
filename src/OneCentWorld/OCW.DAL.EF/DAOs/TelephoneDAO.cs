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
    public class TelephoneDAO : ITelephoneDAO, ITelephoneFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public TelephoneDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region ITelephoneDAO Implemantation
        public Telephone Add(Telephone entity)
        {
            if (entity.Id > 0) return entity;

            context.Telephone.AddObject(entity);

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

        public Telephone Read(int key)
        {
            var listTelephone = context.Telephone.Where(t => key == t.Id);
            if (listTelephone.Count() == 0) throw new RecordNotFoundException<int?>("Telephone", key);
            Telephone telephone = listTelephone.First();

            return telephone;
        }

        public IEnumerable<Telephone> ReadAll(Func<Telephone, bool> predicate = null)
        {
            return context.Telephone.Where(predicate ?? (p => true));
        }

        public IEnumerable<Telephone> ReadAll(int pageSize, int page, Func<Telephone, bool> predicate)
        {
            return context.Telephone.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Telephone, bool> predicate)
        {
            return context.Telephone.Where(predicate ?? (p => true)).Count();
        }

        public Telephone Update(Telephone entity)
        {
           return entity;
        }

        public void Delete(Telephone entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var telephoneList = context.Telephone.Where(t => t.Id == entity);
            if (telephoneList.Count() == 0) throw new RecordNotFoundException<int>("Telephone", entity);
            var telephone = telephoneList.First();

            context.DeleteObject(telephone);
        }

        #endregion

        #region ITelephoneFactory Implementation
        public Telephone Create(int id, string telephone, int profileId)
        {
            Telephone t = context.CreateObject<Telephone>();
            t.Id = id;
            t.Telephone1 = telephone;
            t.Profile_id = profileId;
            return t;
        }
        #endregion
    }
}
