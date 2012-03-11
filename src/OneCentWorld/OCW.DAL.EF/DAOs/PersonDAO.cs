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
    public class PersonDAO : IPersonDAO, IPersonFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public PersonDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IPersonDAO Implementation
        public Person Add(Person entity)
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

        public Person Read(int key)
        {
            var listPerson = context.Profile.OfType<Person>().Where(u => key == u.Id);
            if (listPerson.Count() == 0) return null;
            Person person = listPerson.First();

            return person;
        }

        public IEnumerable<Person> ReadAll(Func<Person, bool> predicate = null)
        {
            return context.Profile.OfType<Person>().Where(predicate ?? (p => true));
        }

        public IEnumerable<Person> ReadAll(int pageSize, int page, Func<Person, bool> predicate)
        {
            return context.Profile.OfType<Person>().Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Person, bool> predicate)
        {
            return context.Profile.OfType<Person>().Where(predicate ?? (p => true)).Count();
        }

        public Person Update(Person entity)
        {
            return entity;
        }

        public void Delete(Person entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var personList = context.Profile.OfType<Person>().Where(p => p.Id == entity);
            if (personList.Count() == 0) throw new RecordNotFoundException<int>("Person", entity);
            var person = personList.First();

            context.DeleteObject(person);
        }
        #endregion

        #region IPersonFactory Implementation
        public Person Create(int id, string email, string password, DateTime registeredDate, int addressId, int accountId, string firstName, string lastName, DateTime? birthDate, int roleId)
        {
            Person p = context.CreateObject<Person>();
            p.Id = id;
            p.Email = email;
            p.Password = password;
            p.RegisteredDate = registeredDate;
            p.Address_id = addressId;
            p.Account_id = accountId;
            p.FirstName = firstName;
            p.LastName = lastName;
            p.Birthdate = birthDate;
            p.Role_id = roleId;
            return p;
        }
        #endregion
    }
}
