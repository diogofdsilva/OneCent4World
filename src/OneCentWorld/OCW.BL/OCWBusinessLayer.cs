using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using OCW.DAL;
using OCW.DAL.DTOs;
using OCW.DAL.EF;
using OCW.DAL.Exceptions;
using OCW.DAL.IndependentLayerEntities;

namespace OCW.BL
{
    public class OCWBusinessLayer
    {
        public static readonly decimal VALOR_CONTRIBUICAO = new decimal(0.01);

        #region Transactions

        public bool Transfer(int from, int to, int organizationId, decimal amount, decimal donation, string reference)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                TransactionOptions options = new TransactionOptions
                                                 {
                                                     IsolationLevel = IsolationLevel.RepeatableRead
                                                 };

                using (TransactionScope tr = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //obter as contas
                    Person person = dal.PersonDAO.Read(from);
                    if (person == null) throw new Exception("From not found!");

                    Company company = dal.CompanyDAO.Read(to);
                    if (company == null) throw new Exception("To not found!");

                    Organization organization = dal.OrganizationDAO.Read(organizationId);

                    Account fromAccount = person.Account;
                    Account toAccount = company.Account;
                    
                    //transferir o dinheiro e guardar 1 centimo para a caridade
                    if (fromAccount.Value > amount + donation)
                        fromAccount.Value -= amount + donation;
                    else throw new Exception("Not enough money!");

                    toAccount.Value += amount;
                    
                    // adicinar o centimo à conta da organização solidaria
                    Account charityAccount = organization.Account;

                    charityAccount.Value += donation;

                    DAL.DTOs.Transaction tranOCW = dal.TransactionFactory.Create(0, amount, donation, DateTime.Now, reference, person.Id, company.Id, organization.Id);
                    
                    dal.TransactionDAO.Add(tranOCW);

                    // fazer update a tudo
                    dal.AccountDAO.Update(fromAccount);
                    dal.AccountDAO.Update(toAccount);
                    dal.AccountDAO.Update(charityAccount);

                    tr.Complete();
                }
            }


            return true;
        }

        public IEnumerable<DAL.DTOs.Transaction> GetTransactionsbyId(int id)
        {
            List<DAL.DTOs.Transaction> transactions = new List<DAL.DTOs.Transaction>();

            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var allTransactions = dal.TransactionDAO.ReadAllFull(w => w.Organization_id == id || w.Company_id == id || w.Person_id == id);

                //if (allTransactions == null)
                //{
                //    return new List<DAL.DTOs.Transaction>().AsEnumerable();
                //}
                
                return allTransactions.ToList(); 
            }
            try
            {
                Company company = GetCompany(id);
                if (company.Transaction.Count > 0) transactions.AddRange(company.Transaction);
            }catch(RecordNotFoundException<int>){}
            
            try
            {
                Organization organization = GetOrganization(id);
                if (organization.Transaction.Count > 0) transactions.AddRange(organization.Transaction);
            }catch(RecordNotFoundException<int>){}
            
            try
            {
                Person person = GetPerson(id);
                if (person.Transaction.Count > 0) transactions.AddRange(person.Transaction);
            }
            catch (RecordNotFoundException<int>) { }

            return transactions;
        }

        public IEnumerable<Movement> GetMovements(int id)
        {
            List<Movement> movs = new List<Movement>();
            Profile p;
            
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                p = dal.ProfileDAO.Read(id);
                movs.AddRange(p.Deposit);
                movs.AddRange(p.Withdraw);
                movs.AddRange(GetTransactionsbyId(id));
            }

            return movs;
        }

        #endregion Transactions

        #region Profile

        public Profile GetProfile(string profileEmail)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Profile profile = dal.ProfileDAO.FindByEmail(profileEmail);

                return profile;
            }
        }

        public Profile GetProfileWithAccount(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Profile p = dal.ProfileDAO.FindByEmail(email);

                p.Account = dal.AccountDAO.Read(p.Account_id);
                return p;
            }
        }

        #endregion Profile

        #region Utils

        public Profile Login(string email, string pass)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var profile = dal.ProfileDAO.FindByEmail(email);

                return profile != null && profile.Password.Equals(pass) ? profile : null;
            }
        }

        public IEnumerable<String> ListAllCountries()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.CountryDAO.ReadAll().Select(o => o.Name).ToList();
            }
        }

        public IEnumerable<Country> GetAllCountries()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.CountryDAO.ReadAll().ToList();
            }
        }
        public string GetProfileType(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return "";
            }
        }

        public IEnumerable<Tag> GetAllTags()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.TagDAO.ReadAll().ToList();
            }
        }

        #endregion Utils

        #region Person

        public void EditPerson(int userId, string firstName, string lastName, string email, string address1, string address2, string postalCode, string city, int countryId, DateTime? birthDate)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.Open();
                using (TransactionScope tr = new TransactionScope())
                {
                    Person p = dal.PersonDAO.Read(userId);
                    p.FirstName = firstName;
                    p.LastName = lastName;
                    p.Email = email;
                    p.Address.Address1 = address1;
                    p.Address.Address2 = address2;
                    p.Address.City = city;
                    p.Address.Country_id = countryId;
                    p.Address.PostalCode = postalCode;
                    p.Birthdate = birthDate;

                    dal.PersonDAO.Update(p);

                    dal.SaveChanges();
                    tr.Complete();
                }
                dal.Close();
            }
        }

        public Person CreatePerson(string firstName, string lastName, string email, string password, string address1, string address2, 
                                        DateTime? birthDate, string postalCode, string city, string country, string role)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Account accountDto = dal.AccountFactory.Create(0, 0);

                    int roleId = dal.RoleDAO.FindByName(role).Id;

                    Address addressDto =
                        dal.AddressFactory.Create(0, address1, address2, postalCode, city, dal.CountryDAO.ReadAll().Single(si => si.Name == country).Id);

                    Person person = dal.PersonFactory.Create(0, email, password, DateTime.Now, addressDto.Id,
                                                        accountDto.Id, firstName, lastName, birthDate, roleId);
                    person.Address = addressDto;
                    person.Account = accountDto;

                    dal.PersonDAO.Add(person);

                    tr.Complete();
                    return person;
                }
            }
        }

        public Person GetPerson(int userId)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.PersonDAO.Read(userId);
            }
        }

        public Person GetPerson(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.ProfileDAO.FindByEmail(email) as Person;
            }
        }

        public Person GetPersonFullInfo(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Person p =  dal.ProfileDAO.FindByEmail(email) as Person;

                p.Address = dal.AddressDAO.Read(p.Address_id);
                p.Address.Country = dal.CountryDAO.Read(p.Address.Country_id);

                return p;
            }
        }

        public void DeletePerson(int person)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.PersonDAO.Delete(person);
            }
        }

        public IEnumerable<DAL.DTOs.Transaction> GetPersonTransactions(string personEmail)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Person person = dal.ProfileDAO.FindByEmail(personEmail) as Person;
                    return person == null ? null : person.Transaction;
                }
            }
        }

        #endregion Person

        #region Company

        public void EditCompany(int id, string name, string url, byte[] image, string email, string address1, string address2, string postalCode, string city, int countryId, List<int> tags)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.Open();
                using (TransactionScope tr = new TransactionScope())
                {
                    Company c = dal.CompanyDAO.Read(id);
                    c.Name = name;
                    c.Url = url;
                    c.Email = email;
                    c.Address.Address1 = address1;
                    c.Address.Address2 = address2;
                    c.Address.City = city;
                    c.Address.PostalCode = postalCode;
                    c.Address.Country_id = countryId;
                    if (image != null) c.Image = image;

                    List<Tag> removeTags = new List<Tag>();

                    foreach (var tag in c.Tag)
                    {
                        if (tags.Contains(tag.Id)) tags.Remove(tag.Id);
                        else removeTags.Add(tag);
                    }

                    foreach (var tag in removeTags)
                    {
                        c.Tag.Remove(tag);
                    }

                    foreach (var tag in tags)
                    {
                        Tag t = dal.TagDAO.Read(tag);
                        c.Tag.Add(t);
                    }

                    dal.CompanyDAO.Update(c);

                    dal.SaveChanges();

                    tr.Complete();
                }
                dal.Close();
            }
        }

        public Company CreateCompany(string name, string url, byte[] image,
            string email, string password,
            string address1, string address2, string postalCode, string city,
            int country, IEnumerable<int> tagIds)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    

                    Account account = dal.AccountFactory.Create(0, 0);
                    dal.AccountDAO.Add(account);

                    Address address =
                        dal.AddressFactory.Create(0, address1, address2, postalCode, city, country);
                    dal.AddressDAO.Add(address);

                    Company company = dal.CompanyFactory.Create(0, email, password, DateTime.Now, address.Id,
                                                        account.Id, name, image, url, tagIds);

                    company.Address = address;
                    company.Account = account;

                    dal.CompanyDAO.Add(company);

                    tr.Complete();
                    return company;
                }


            }
        }

        public Company GetCompany(int companyId)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.CompanyDAO.Read(companyId);
            }
        }

        public Company GetCompany(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.ProfileDAO.FindByEmail(email) as Company;
            }
        }

        public Company GetCompanyFullInfo(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Company c = dal.CompanyDAO.ReadAll(where => where.Email == email).First();

                c.Address = dal.AddressDAO.Read(c.Address_id);
                c.Address.Country = dal.CountryDAO.Read(c.Address.Country_id);
                c.Tag.ToList();

                return c;
            }
        }

        public void DeleteCompany(int company)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.CompanyDAO.Delete(company);
            }
        }

        public IEnumerable<DAL.DTOs.Transaction> GetCompanyTransactions(string companyEmail)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Company company = dal.ProfileDAO.FindByEmail(companyEmail) as Company;
                    return company == null ? null : company.Transaction;
                }
            }
        }

        #endregion Company

        #region Organization

        public void EditOrganization(int id, string name, string url, byte[] image, string email, string address1, string address2, string postalCode, string city, int countryId, List<int> tags)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.Open();
                using (TransactionScope tr = new TransactionScope())
                {
                    Organization o = dal.OrganizationDAO.Read(id);
                    o.Name = name;
                    o.Url = url;
                    o.Email = email;
                    o.Address.Address1 = address1;
                    o.Address.Address2 = address2;
                    o.Address.City = city;
                    o.Address.PostalCode = postalCode;
                    o.Address.Country_id = countryId;
                    if (image != null) o.Image = image;

                    List<Tag> removeTags = new List<Tag>();

                    foreach (var tag in o.Tag)
                    {
                        if (tags.Contains(tag.Id)) tags.Remove(tag.Id);
                        else removeTags.Add(tag);
                    }

                    foreach (var tag in removeTags)
                    {
                        o.Tag.Remove(tag);
                    }

                    foreach (var tag in tags)
                    {
                        Tag t = dal.TagDAO.Read(tag);
                        o.Tag.Add(t);
                    }

                    dal.OrganizationDAO.Update(o);

                    dal.SaveChanges();

                    tr.Complete();
                }
                dal.Close();
            }
        }

        public IEnumerable<Organization> GetAllOrganizations()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.OrganizationDAO.ReadAll().ToList();
            }
        }

        public IndependentOngProfile GetOng(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var profile = dal.ProfileDAO.FindByEmail(email);
                var ong= dal.OrganizationDAO.Read(profile.Id);
                return profile != null ?
                    new IndependentOngProfile()
                    {
                        Address1 = profile.Address.Address1,
                        Address2 = profile.Address.Address2,
                        City = profile.Address.City,
                        Country = profile.Address.Country.Id,
                        Email = profile.Email,
                        Name = profile.ProfileName,
                        Password = profile.Password,
                        PostalCode = profile.Address.PostalCode,
                        Url = ong.Url,
                        //TypeId = ong.Tag,
                    }
                    : null;
            }
        }

        public Organization CreateOrganization(string name, string url, byte[] image, string email, string password,
            string address1, string address2, string postalCode, string city, int countryId, IEnumerable<int> tagIds)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Account account = dal.AccountFactory.Create(0, 0);
                    dal.AccountDAO.Add(account);

                    Address address = dal.AddressFactory.Create(0, address1, address2, postalCode, city, countryId);
                    dal.AddressDAO.Add(address);

                    Organization organization = dal.OrganizationFactory.Create(0, 
                                                                       email,
                                                                       password,
                                                                       DateTime.Now,
                                                                       address.Id,
                                                                       account.Id,
                                                                       name,
                                                                       image,
                                                                       url,
                                                                       tagIds);

                    organization.Address = address;
                    organization.Account = account;

                    dal.OrganizationDAO.Add(organization);

                    tr.Complete();
                    return organization;
                }
            }
        }

        public Organization GetOrganization(int organizationId)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.OrganizationDAO.Read(organizationId);
            }
        }

        public Organization GetOrganization(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.ProfileDAO.FindByEmail(email) as Organization;
            }
        }

        public Organization GetOrganizationFullInfo(string email)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Organization o = dal.OrganizationDAO.ReadAll(where => where.Email == email).First();

                o.Address = dal.AddressDAO.Read(o.Address_id);
                o.Address.Country = dal.CountryDAO.Read(o.Address.Country_id);
                o.Tag.ToList();

                return o;
            }
        }

        public Organization GetOrganizationFromCompany(int companyId)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Organization org = null;

                IEnumerable<Emergency> emergencies = GetActiveEmergencies();
                foreach (var emergency in emergencies)
                {
                    Random random = new Random();
                    int next = random.Next(0, 100);
                    if (next <= emergency.Weight * 100) return emergency.Organization;
                }
                
                Company c = dal.CompanyDAO.Read(companyId);

                List<Organization> o = new List<Organization>();
                foreach (Tag tag in c.Tag)
                {
                    foreach (Organization organization in tag.Organization)
                    {
                        if(!o.Contains(organization)) o.Add(organization);
                    }
                }

                return o.Count > 0 ? o.Random() : dal.OrganizationDAO.ReadAll().ToList().Random();
            }
        }

        public void DeleteOrganization(int organization)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.OrganizationDAO.Delete(organization);
            }
        }

        public IEnumerable<DAL.DTOs.Transaction> GetOrganizationTransactions(string organizationEmail)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                Organization organization = dal.ProfileDAO.FindByEmail(organizationEmail) as Organization;
                
                return organization == null ? null : organization.Transaction;
            }
        }

        public IEnumerable<string> ListAllOrganizations()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.OrganizationDAO.ReadAll().Select(o => o.Name).ToList();
            }
        }

        public IEnumerable<IndependentOrganization> ListAllOrganizationsIndependentEntities()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                var orgs = dal.OrganizationDAO.ReadAll();
                List<IndependentOrganization> io = new List<IndependentOrganization>();
                foreach (var org in orgs)
                {
                    io.Add(new IndependentOrganization()
                    {
                        Name = org.Name,
                        Id = org.Id,
                    }
                    );
                }
                return io;                                       
            }
        }

        #endregion Organization

        #region Deposit

        public bool Deposit(string toEmail, decimal amount, string source)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    //obter as contas

                    Profile profile = dal.ProfileDAO.FindByEmail(toEmail);
                    if (profile == null) return false;

                    Account toAccount = profile.Account;

                    //transferir o dinheiro
                    toAccount.Value += amount;


                    // fazer update a tudo
                    dal.AccountDAO.Update(toAccount);

                    // adicionar deposito

                    var deposit = dal.DepositFactory.Create(0, DateTime.Now, amount, source, profile.Id);

                    dal.DepositDAO.Add(deposit);

                    tr.Complete();
                }
            }

            return true;
        }

        public IEnumerable<Deposit> GetDepositList(string name)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                int profileId = dal.ProfileDAO.FindByEmail(name).Id;
                return dal.DepositDAO.ReadAll().Where(w => w.Profile_id == profileId).ToList();
            }
        }

        #endregion Deposit

        #region Withdraw
        public bool Withdraw(int from, decimal amount, string destination)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                TransactionOptions options = new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.RepeatableRead
                };

                using (TransactionScope tr = new TransactionScope(TransactionScopeOption.Required, options))
                {
                    //obter as contas
                    Profile profile = dal.ProfileDAO.Read(from);
                    Account fromAccount = profile.Account;

                    //remover o dinheiro
                    if (fromAccount.Value >= amount)
                    {
                        fromAccount.Value -= amount;
                        dal.AccountDAO.Update(fromAccount);
                        var wtd = dal.WithdrawFactory.Create(0, DateTime.Now, amount, destination, from);
                        dal.WithdrawDAO.Add(wtd);
                    }
                    else return false;

                    tr.Complete();
                }
            }
            return true;
        }

        public IEnumerable<Withdraw> GetWithdrawList(string name)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                int profileId = dal.ProfileDAO.FindByEmail(name).Id;
                return dal.WithdrawDAO.ReadAll().Where(w => w.Profile_id == profileId).ToList();
            }
        }
        #endregion

        #region Emergency
        public void EditEmergency(int id, string title, string description, DateTime startDate, DateTime? endDate, decimal weight, byte[] tempImage, int organization)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Emergency e = dal.EmergencyDAO.Read(id);
                    e.Title = title;
                    e.Description = description;
                    e.StartDate = startDate;
                    e.EndDate = endDate;
                    e.Weight = weight;
                    e.Organization_id = organization;
                    if (tempImage != null) e.Image = tempImage;

                    dal.SaveChanges();
                    tr.Complete();
                }
            }
        }

        public IEnumerable<Emergency> GetEmergency(int pageSize, int page)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                    return dal.EmergencyDAO.ReadAll(pageSize, page).ToList();
            }
        }

        public IEnumerable<Emergency> GetEmergencyWithOrganization(int pageSize, int page)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                IEnumerable<Emergency> emergencies = dal.EmergencyDAO.ReadAll(pageSize, page);
                foreach (var emergency in emergencies)
                {
                    Organization o = emergency.Organization;
                }

                return emergencies.ToList();
            }
        }

        public Emergency GetEmergency(int id)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                return dal.EmergencyDAO.Read(id);
            }
        }

        public void CreateEmergency(string title, string description, DateTime startDate, DateTime? endDate, decimal weight, byte[] image, int organizationId)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    Emergency e = dal.EmergencyFactory.Create(0, title, description, startDate, endDate, weight, image, organizationId);
                    dal.EmergencyDAO.Add(e);
                    tr.Complete();
                }
            }
        }

        public IEnumerable<Emergency> GetActiveEmergencies()
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                using (TransactionScope tr = new TransactionScope())
                {
                    DateTime now = DateTime.Now;
                    IEnumerable<Emergency> e = dal.EmergencyDAO.ReadAll(r => r.StartDate.CompareTo(now) < 0 && (r.EndDate == null || r.EndDate.Value.CompareTo(now) > 0)).ToList();
                    tr.Complete();
                    return e;
                }
            }
        }

        public void DeleteEmergency(int id)
        {
            using (DataAccessLayer dal = new EFDataAccessLayer())
            {
                dal.EmergencyDAO.Delete(id);
                dal.SaveChanges();
            }
        }
        #endregion

    }

    //inutil
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
