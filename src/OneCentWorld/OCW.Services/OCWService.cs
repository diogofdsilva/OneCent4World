using System;
using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using OCW.Services.Interfaces;
using OCW.DAL;
using OCW.DAL.EF;
using System.Transactions;
using OCW.DAL.DTOs;
using System.Linq;

namespace OCW.Services
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple,InstanceContextMode = InstanceContextMode.Single)]
    public class OCWService : IOCWService
    {
        public static readonly decimal VALOR_CONTRIBUICAO = new decimal(0.01);

        private Hashtable currTickets;

        public OCWService()
        {
            this.currTickets = new Hashtable();
        }

        #region Implementation of IOCWService

        public Guid Login(int userID, string pass)
        {
            // Validar utilizador
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var user = dal.PersonDAO.Read(userID);


                if (user == null || !user.Password.Equals(pass)) 
                {
                    return Guid.Empty;
                }
            }


            // Em caso afirmativo, criar o ticket
            Ticket t = new Ticket(Guid.NewGuid(), DateTime.Now.AddMinutes(2));
            
            currTickets.Add(t.TicketId,t);

            return t.TicketId;
        }

        public Guid Login(string user, string pass)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var userDto = dal.PersonDAO.ReadAll().SingleOrDefault(o => o.Email == user);

                if (userDto == null || !userDto.Password.Equals(pass)) 
                {
                    return Guid.Empty;
                }
            }

            // Em caso afirmativo, criar o ticket
            Ticket t = new Ticket(Guid.NewGuid(), DateTime.Now.AddMinutes(2));
            
            currTickets.Add(t.TicketId,t);

            return t.TicketId;
        }

        public bool Transfer(Guid ticket, int from, int to, decimal ammount, string[] tags)
        {
            // Validar o ticket
            if (!ValidateTicket(ticket))
            {
                return false;
            }

            using(DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope()) 
                {
                    //obter as contas

                    AccountDTO fromAccount = dal.AccountDAO.Read(from);
                    AccountDTO toAccount = dal.AccountDAO.Read(to);

                    //transferir o dinheiro
                    fromAccount.Value -= ammount;
                    toAccount.Value += ammount;
                    
                    //guardar 1Centimo para a caridade
                    fromAccount.Value -= VALOR_CONTRIBUICAO;


                    // adicinar o centimo à conta da organização solidaria


                    // Adicionar transacçao - isto vai demorar longitudes de tempo... uma msmq a fazer isto é melhor
                    ////TODO Falta acabar isto
                    TransactionDTO tranOCW = new TransactionDTO(0,ammount, VALOR_CONTRIBUICAO, DateTime.Now, 0,0,0);
                    dal.TransactionDAO.Create(tranOCW);


                    // fazer update a tudo
                    dal.AccountDAO.Update(fromAccount);
                    dal.AccountDAO.Update(toAccount);

                    tr.Complete();
                }
            }


            return true;
        }




        public void CreateUser(PersonDTO person)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                AccountDTO accountDto = dal.AccountDAO.Create(new AccountDTO(null, 0, dal));
                //piwi devia ser assim (bem mais simples): person.Account = accountDto;
                //person.AccountID = accountDto.ID.GetValueOrDefault(0);    
                
                dal.PersonDAO.Create(person);
            }
        }

        public PersonDTO GetUser(Guid ticket, int userId)
        {
            // Validar o ticket
            if (!ValidateTicket(ticket))
            {
                //throw exception
                //throw new AccessViolationException();
                return null;
            }

            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.PersonDAO.Read(userId);
            }
        }

        public PersonDTO GetUser(Guid ticket, string user)
        {
            // Validar o ticket
            if (!ValidateTicket(ticket))
            {
                //throw exception
                //throw new AccessViolationException();
                return null;
            }

            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.PersonDAO.ReadAll().SingleOrDefault( s => s.Email == user);
            }
        }

        public void DeleteUser(Guid ticket, int user)
        {
            // Validar o ticket
            if (!ValidateTicket(ticket))
            {
                //throw exception
                //throw new AccessViolationException();
                return;
            }

            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.PersonDAO.Delete(user);
            }
        }

        #endregion

        private bool ValidateTicket(Guid ticket)
        {
            Ticket g = (Ticket) currTickets[ticket];

            if(g.Expires.CompareTo(DateTime.Now) > 0)
            {
                // remover o ticket
                currTickets.Remove(ticket);
                return false;
            }

            return true;
        }

        
    }

    internal struct Ticket
    {
        private Guid ticketId;
        private DateTime expires;

        public Ticket(Guid ticket, DateTime expires)
        {
            this.ticketId = ticket;
            this.expires = expires;
        }

        public Guid TicketId
        {
            get { return ticketId; }
            set { ticketId = value; }
        }

        public DateTime Expires
        {
            get { return expires; }
            set { expires = value; }
        }
    }
}