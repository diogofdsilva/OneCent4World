using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs;
using OCW.DAL.DTOs.Factory;
using OCW.DAL.Exceptions;

namespace OCW.DAL.EF.DAOs
{
    public class AccountDAO : IAccountDAO, IAccountFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public AccountDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IAccountDAO Implementation
        public Account Add(Account entity)
        {
            if (entity.Id > 0) return entity;
            context.Account.AddObject(entity);

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

        public Account Read(int key)
        {
            var listAccount = context.Account.Where(a => key == a.Id);
            if(listAccount.Count() == 0) throw new RecordNotFoundException<int?>("Account", key);
            Account account = listAccount.First();

            return account;
        }

        public IEnumerable<Account> ReadAll(Func<Account, bool> predicate = null)
        {
            return context.Account.Where(predicate ?? (p => true));
        }

        public IEnumerable<Account> ReadAll(int pageSize, int page, Func<Account, bool> predicate)
        {
            return context.Account.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Account, bool> predicate)
        {
            return context.Account.Where(predicate ?? (p => true)).Count();
        }

        public Account Update(Account entity)
        {
            return entity;
        }

        public void Delete(Account entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var accountList = context.Account.Where(a => a.Id == entity);
            if (accountList.Count() == 0) throw new RecordNotFoundException<int>("Account", entity);
            var account = accountList.First();

            context.DeleteObject(account);
        }

        #endregion

        #region IAccountFactory Implementation
        public Account Create(int id, decimal value)
        {
            Account a = context.CreateObject<Account>();
            a.Id = id;
            a.Value = value;
            return a;
        }
        #endregion
    }
}
