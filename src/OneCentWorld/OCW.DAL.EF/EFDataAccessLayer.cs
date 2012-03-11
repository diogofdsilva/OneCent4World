using System;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs.Factory;
using OCW.DAL.EF.DAOs;
using System.Data;
using OCW.DAL.Exceptions;
using ConfigurationException = OCW.DAL.Exceptions.ConfigurationException;

namespace OCW.DAL.EF
{
    public class EFDataAccessLayer : DataAccessLayer
    {
        #region Readonly Variables
        private readonly OCWEntities context = new OCWEntities();
        #endregion

        #region Variables
        private OrganizationDAO organizationDAO;
        private TagDAO _tagDAO;
        private PersonDAO personDAO;
        private ProfileDAO profileDAO;
        private TelephoneDAO telephoneDAO;
        private RoleDAO roleDAO;
        private EmergencyDAO emergencyDAO;
        private TransactionDAO transactionDAO;
        private CountryDAO countryDAO;
        private DepositDAO depositDAO;
        private WithdrawDAO withdrawDAO;
        private AccountDAO accountDAO;
        private CompanyDAO companyDAO;
        private AddressDAO addressDAO;
        #endregion

        #region DAO Properties
        public override IOrganizationDAO OrganizationDAO
        {
            get 
            {
                lock (this)
                {
                    return organizationDAO ?? (organizationDAO = new OrganizationDAO(context));
                }
            }
        }

        public override ITagDAO TagDAO
        {
            get
            {
                lock (this)
                {
                    return _tagDAO ?? (_tagDAO = new TagDAO(context));
                }
            }
        }

        public override IPersonDAO PersonDAO
        {
            get 
            {
                lock (this)
                {
                    return personDAO ?? (personDAO = new PersonDAO(context));
                }
            }
        }

        public override ITelephoneDAO TelephoneDAO
        {
            get
            {
                lock (this)
                {
                    return telephoneDAO ?? (telephoneDAO = new TelephoneDAO(context));
                }
            }
        }

        public override IAddressDAO AddressDAO
        {
            get
            {
                lock (this)
                {
                    return addressDAO ?? (addressDAO = new AddressDAO(context));
                }
            }
        }

        public override ICompanyDAO CompanyDAO
        {
            get
            {
                lock (this)
                {
                    return companyDAO ?? (companyDAO = new CompanyDAO(context));
                }
            }
        }

        public override IAccountDAO AccountDAO
        {
            get
            {
                lock (this)
                {
                    return accountDAO ?? (accountDAO = new AccountDAO(context));
                }
            }
        }

        public override ICountryDAO CountryDAO
        {
            get
            {
                lock (this)
                {
                    return countryDAO ?? (countryDAO = new CountryDAO(context));
                }
            }
        }

        public override IDepositDAO DepositDAO
        {
            get
            {
                lock (this)
                {
                    return depositDAO ?? (depositDAO = new DepositDAO(context));
                }
            }
        }

        public override IWithdrawDAO WithdrawDAO
        {
            get
            {
                lock (this)
                {
                    return withdrawDAO ?? (withdrawDAO = new WithdrawDAO(context));
                }
            }
        }

        public override ITransactionDAO TransactionDAO
        {
            get
            {
                lock (this)
                {
                    return transactionDAO ?? (transactionDAO = new TransactionDAO(context));
                }
            }
        }

        public override IEmergencyDAO EmergencyDAO
        {
            get
            {
                lock (this)
                {
                    return emergencyDAO ?? (emergencyDAO = new EmergencyDAO(context));
                }
            }
        }

        public override IRoleDAO RoleDAO
        {
            get
            {
                lock (this)
                {
                    return roleDAO ?? (roleDAO = new RoleDAO(context));
                }
            }
        }

        public override IProfileDAO ProfileDAO
        {
            get
            {
                lock (this)
                {
                    return profileDAO ?? (profileDAO = new ProfileDAO(context));
                }
            }
        }
        #endregion
        
        #region DTO Factories Properties
        public override IAccountFactory AccountFactory
        {
            get
            {
                lock (this)
                {
                    return accountDAO ?? (accountDAO = new AccountDAO(context));
                }
            }
        }

        public override IAddressFactory AddressFactory
        {
            get
            {
                lock (this)
                {
                    return addressDAO ?? (addressDAO = new AddressDAO(context));
                }
            }
        }

        public override ICompanyFactory CompanyFactory
        {
            get
            {
                lock (this)
                {
                    return companyDAO ?? (companyDAO = new CompanyDAO(context));
                }
            }
        }

        public override ICountryFactory CountryFactory
        {
            get
            {
                lock (this)
                {
                    return countryDAO ?? (countryDAO = new CountryDAO(context));
                }
            }
        }

        public override IDepositFactory DepositFactory
        {
            get
            {
                lock (this)
                {
                    return depositDAO ?? (depositDAO = new DepositDAO(context));
                }
            }
        }

        public override IEmergencyFactory EmergencyFactory
        {
            get
            {
                lock (this)
                {
                    return emergencyDAO ?? (emergencyDAO = new EmergencyDAO(context));
                }
            }
        }

        public override IOrganizationFactory OrganizationFactory
        {
            get
            {
                lock (this)
                {
                    return organizationDAO ?? (organizationDAO = new OrganizationDAO(context));
                }
            }
        }

        public override ITagFactory TagFactory
        {
            get
            {
                lock (this)
                {
                    return _tagDAO ?? (_tagDAO = new TagDAO(context));
                }
            }
        }

        public override IPersonFactory PersonFactory
        {
            get
            {
                lock (this)
                {
                    return personDAO ?? (personDAO = new PersonDAO(context));
                }
            }
        }

        public override IRoleFactory RoleFactory
        {
            get
            {
                lock (this)
                {
                    return roleDAO ?? (roleDAO = new RoleDAO(context));
                }
            }
        }

        public override ITelephoneFactory TelephoneFactory
        {
            get
            {
                lock (this)
                {
                    return telephoneDAO ?? (telephoneDAO = new TelephoneDAO(context));
                }
            }
        }

        public override ITransactionFactory TransactionFactory
        {
            get
            {
                lock (this)
                {
                    return transactionDAO ?? (transactionDAO = new TransactionDAO(context));
                }
            }
        }

        public override IWithdrawFactory WithdrawFactory
        {
            get
            {
                lock (this)
                {
                    return withdrawDAO ?? (withdrawDAO = new WithdrawDAO(context));
                }
            }
        }
        #endregion

        #region Methods
        public override void Open()
        {
            try
            {
                context.Connection.Open();
            }
            catch (InvalidOperationException e)
            {
                //An error occurs when you open the connection, or the name of the underlying data provider is not known.
                throw new ConnectionException(e.Message);
            }
            catch (MetadataException e)
            {
                //The inline connection string contains an invalid Metadata keyword value.
                throw new ConfigurationException(e.Message);
            }
        }

        public override void SaveChanges()
        {
            try
            {
                context.SaveChanges();
            }
            catch (OptimisticConcurrencyException e)
            {
                //An optimistic concurrency violation has occurred in the data source.
                throw new ConcurrencyException(e.Message);
            }
        }

        public override void Close()
        {
            try
            {
                context.Connection.Close();
            }
            catch (InvalidOperationException e)
            {
                //An error occurred when closing the connection.
                throw;
            }
        }

        public override void Dispose()
        {
            context.Dispose();
        }
        #endregion
    }
}
