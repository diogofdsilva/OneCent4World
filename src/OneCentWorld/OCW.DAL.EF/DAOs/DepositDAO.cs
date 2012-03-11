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
    public class DepositDAO : IDepositDAO, IDepositFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public DepositDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IDepositDAO Implementation
        public Deposit Add(Deposit entity)
        {
            if (entity.Id > 0) return entity;
            context.Deposit.AddObject(entity);

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

        public Deposit Read(int key)
        {
            var listDeposit = context.Deposit.Where(d => key == d.Id);
            if(listDeposit.Count() == 0) throw new RecordNotFoundException<int?>("Deposit", key);
            Deposit deposit = listDeposit.First();

            return deposit;
        }

        public IEnumerable<Deposit> ReadAll(Func<Deposit, bool> predicate = null)
        {
            return context.Deposit.Where(predicate ?? (p => true));
        }

        public IEnumerable<Deposit> ReadAll(int pageSize, int page, Func<Deposit, bool> predicate)
        {
            return context.Deposit.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Deposit, bool> predicate)
        {
            return context.Deposit.Where(predicate ?? (p => true)).Count();
        }

        public Deposit Update(Deposit entity)
        {
            return entity;
        }

        public void Delete(Deposit entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var depositList = context.Deposit.Where(d => d.Id == entity);
            if (depositList.Count() == 0) throw new RecordNotFoundException<int>("Deposit", entity);
            var deposit = depositList.First();

            context.DeleteObject(deposit);
        }

        #endregion

        #region IDepositFactory Implementation
        public Deposit Create(int id, DateTime date, decimal amount, string source, int profileId)
        {
            Deposit d = context.CreateObject<Deposit>();
            d.Id = id;
            d.Date = date;
            d.Amount = amount;
            d.Source = source;
            d.Profile_id = profileId;
            return d;
        }
        #endregion
    }
}
