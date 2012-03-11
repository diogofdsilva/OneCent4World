using System;
using OCW.DAL.DAOs;
using OCW.DAL.DTOs.Factory;

namespace OCW.DAL
{
    public abstract class DataAccessLayer : IDisposable
    {
        #region DAO Properties
        public abstract IAccountDAO AccountDAO { get; }
        public abstract IAddressDAO AddressDAO { get; }
        public abstract ICompanyDAO CompanyDAO { get; }
        public abstract ICountryDAO CountryDAO { get; }
        public abstract IDepositDAO DepositDAO { get; }
        public abstract IEmergencyDAO EmergencyDAO { get; }
        public abstract IOrganizationDAO OrganizationDAO { get; }
        public abstract ITagDAO TagDAO { get; }
        public abstract IPersonDAO PersonDAO { get; }
        public abstract IProfileDAO ProfileDAO { get; }
        public abstract IRoleDAO RoleDAO { get; }
        public abstract ITelephoneDAO TelephoneDAO { get; }
        public abstract ITransactionDAO TransactionDAO { get; }
        public abstract IWithdrawDAO WithdrawDAO { get; }
        #endregion

        #region DTO Factories Properties
        public abstract IAccountFactory AccountFactory { get; }
        public abstract IAddressFactory AddressFactory { get; }
        public abstract ICompanyFactory CompanyFactory { get; }
        public abstract ICountryFactory CountryFactory { get; }
        public abstract IDepositFactory DepositFactory { get; }
        public abstract IEmergencyFactory EmergencyFactory { get; }
        public abstract IOrganizationFactory OrganizationFactory { get; }
        public abstract ITagFactory TagFactory { get; }
        public abstract IPersonFactory PersonFactory { get; }
        public abstract IRoleFactory RoleFactory { get; }
        public abstract ITelephoneFactory TelephoneFactory { get; }
        public abstract ITransactionFactory TransactionFactory { get; }
        public abstract IWithdrawFactory WithdrawFactory { get; }
        #endregion

        #region Methods
        public abstract void Open();
        public abstract void SaveChanges();
        public abstract void Close();
        public abstract void Dispose();
        #endregion
    }
}
