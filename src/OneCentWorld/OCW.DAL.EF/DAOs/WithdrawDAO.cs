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
    public class WithdrawDAO : IWithdrawDAO, IWithdrawFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public WithdrawDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region IWithdrawDAO Implementation
        public Withdraw Add(Withdraw entity)
        {
            if (entity.Id > 0) return entity;
            context.Withdraw.AddObject(entity);

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

        public Withdraw Read(int key)
        {
            var listWithdraw = context.Withdraw.Where(w => key == w.Id);
            if (listWithdraw.Count() == 0) throw new RecordNotFoundException<int?>("Withdraw", key);
            Withdraw withdraw = listWithdraw.First();

            return withdraw;
        }

        public IEnumerable<Withdraw> ReadAll(Func<Withdraw, bool> predicate = null)
        {
            return context.Withdraw.Where(predicate ?? (p => true));
        }

        public IEnumerable<Withdraw> ReadAll(int pageSize, int page, Func<Withdraw, bool> predicate)
        {
            return context.Withdraw.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Withdraw, bool> predicate)
        {
            return context.Withdraw.Where(predicate ?? (p => true)).Count();
        }

        public Withdraw Update(Withdraw entity)
        {
            return entity;
        }

        public void Delete(Withdraw entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var withdrawList = context.Withdraw.Where(w => w.Id == entity);
            if (withdrawList.Count() == 0) throw new RecordNotFoundException<int>("Withdraw", entity);
            var withdraw = withdrawList.First();

            context.DeleteObject(withdraw);
        }

        #endregion

        #region IWithdrawFactory Implementation
        public Withdraw Create(int id, DateTime date, decimal amount, string destination, int profileId)
        {
            Withdraw w = context.CreateObject<Withdraw>();
            w.Id = id;
            w.Date = date;
            w.Amount = amount;
            w.Destination = destination;
            w.Profile_id = profileId;
            return w;
        }
        #endregion
    }
}
