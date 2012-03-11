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
    public class TransactionDAO : ITransactionDAO, ITransactionFactory
    {
        #region Local Variables
        private readonly OCWEntities context;
        #endregion

        #region Constructors
        public TransactionDAO(OCWEntities context)
        {
            this.context = context;
        }
        #endregion

        #region ITransactionDAO Implementation
        public Transaction Add(Transaction entity)
        {
            if (entity.Id > 0) return entity;

            context.Transaction.AddObject(entity);

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

        public Transaction Read(int key)
        {
            var listTransaction = context.Transaction.Where(t => key == t.Id);
            if(listTransaction.Count() == 0) throw new RecordNotFoundException<int?>("Transaction", key);
            Transaction transaction = listTransaction.First();

            return transaction;
        }

        public IEnumerable<Transaction> ReadAll(Func<Transaction, bool> predicate = null)
        {
            return context.Transaction.Where(predicate ?? (p => true));
        }


        public IEnumerable<Transaction> ReadAllFull(Func<Transaction, bool> predicate = null)
        {
            return context.Transaction.Include("Organization").Include("Person").Include("Company").Where(predicate ?? (p => true));
        }


        public IEnumerable<Transaction> ReadAll(int pageSize, int page, Func<Transaction, bool> predicate)
        {
            return context.Transaction.Where(predicate ?? (p => true)).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public int Count(Func<Transaction, bool> predicate)
        {
            return context.Transaction.Where(predicate ?? (p => true)).Count();
        }

        public Transaction Update(Transaction entity)
        {
           return entity;
        }

        public void Delete(Transaction entity)
        {
            context.DeleteObject(entity);
        }

        public void Delete(int entity)
        {
            var transactionList = context.Transaction.Where(t => t.Id == entity);
            if (transactionList.Count() == 0) throw new RecordNotFoundException<int>("Transaction", entity);
            var transaction = transactionList.First();

            context.DeleteObject(transaction);
        }

        #endregion

        #region ITransactionFactory Implementation
        public Transaction Create(int id, decimal value, decimal donation, DateTime date, string reference, int personId, int companyId, int? organizationId)
        {
            Transaction t = context.CreateObject<Transaction>();
            t.Id = id;
            t.Value = value;
            t.Donation = donation;
            t.Date = date;
            t.Person_id = personId;
            t.Company_id = companyId;
            t.Organization_id = organizationId;
            t.Reference = reference;
            return t;
        }
        #endregion
    }
}
