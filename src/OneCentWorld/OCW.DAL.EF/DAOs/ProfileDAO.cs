using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs;
using OCW.DAL.Exceptions;

namespace OCW.DAL.EF.DAOs
{
    public class ProfileDAO : IProfileDAO
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public ProfileDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IProfileDAO Implementation
        public Profile Add(Profile entity)
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

        public Profile Read(int key)
        {
            var listProfile = context.Profile.Where(p => key == p.Id);
            if (listProfile.Count() == 0) throw new RecordNotFoundException<int?>("Profile", key);
            Profile profile = listProfile.First();

            return profile;
        }

        public IEnumerable<Profile> ReadAll(Func<Profile, bool> predicate = null)
        {
            return context.Profile.Where(predicate ?? (p => true));
        }

        public IEnumerable<Profile> ReadAll(int pageSize, int page, Func<Profile, bool> predicate)
        {
            return context.Profile.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Profile, bool> predicate)
        {
            return context.Profile.Where(predicate ?? (p => true)).Count();
        }

        public Profile Update(Profile entity)
        {
            return entity;
        }

        public void Delete(Profile entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var accountProfile = context.Profile.Where(p => p.Id == entity);
            if (accountProfile.Count() == 0) throw new RecordNotFoundException<int>("Profile", entity);
            var profile = accountProfile.First();

            context.DeleteObject(profile);
        }

        public Profile FindByEmail(string email)
        {
            var emailList = context.Profile.Where(p => p.Email.ToLower().Equals(email.ToLower()));
            if (emailList.Count() == 0) return null;

            return emailList.First();
        }
        #endregion   
    }
}
