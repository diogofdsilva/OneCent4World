using System.Data.Objects;
using System.Data.EntityClient;
using OCW.DAL.DTOs;

namespace OCW.DAL.EF
{
    public partial class OCWEntities : ObjectContext
    {
        public const string ConnectionString = "name=OCWEntities";
        public const string ContainerName = "OCWEntities";

        #region Constructors

        public OCWEntities()
            : base(ConnectionString, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }

        public OCWEntities(string connectionString)
            : base(connectionString, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }

        public OCWEntities(EntityConnection connection)
            : base(connection, ContainerName)
        {
            this.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion

        #region ObjectSet Properties

        public ObjectSet<Account> Account
        {
            get { return _account ?? (_account = CreateObjectSet<Account>("Account")); }
        }
        private ObjectSet<Account> _account;

        public ObjectSet<Address> Address
        {
            get { return _address ?? (_address = CreateObjectSet<Address>("Address")); }
        }
        private ObjectSet<Address> _address;

        public ObjectSet<Country> Country
        {
            get { return _country ?? (_country = CreateObjectSet<Country>("Country")); }
        }
        private ObjectSet<Country> _country;

        public ObjectSet<Emergency> Emergency
        {
            get { return _emergency ?? (_emergency = CreateObjectSet<Emergency>("Emergency")); }
        }
        private ObjectSet<Emergency> _emergency;

        public ObjectSet<Profile> Profile
        {
            get { return _profile ?? (_profile = CreateObjectSet<Profile>("Profile")); }
        }
        private ObjectSet<Profile> _profile;

        public ObjectSet<Role> Role
        {
            get { return _role ?? (_role = CreateObjectSet<Role>("Role")); }
        }
        private ObjectSet<Role> _role;

        public ObjectSet<Telephone> Telephone
        {
            get { return _telephone ?? (_telephone = CreateObjectSet<Telephone>("Telephone")); }
        }
        private ObjectSet<Telephone> _telephone;

        public ObjectSet<Transaction> Transaction
        {
            get { return _transaction ?? (_transaction = CreateObjectSet<Transaction>("Transaction")); }
        }
        private ObjectSet<Transaction> _transaction;

        public ObjectSet<Deposit> Deposit
        {
            get { return _deposit ?? (_deposit = CreateObjectSet<Deposit>("Deposit")); }
        }
        private ObjectSet<Deposit> _deposit;

        public ObjectSet<Tag> Tag
        {
            get { return _tag ?? (_tag = CreateObjectSet<Tag>("Tag")); }
        }
        private ObjectSet<Tag> _tag;

        public ObjectSet<Withdraw> Withdraw
        {
            get { return _withdraw ?? (_withdraw = CreateObjectSet<Withdraw>("Withdraw")); }
        }
        private ObjectSet<Withdraw> _withdraw;

        #endregion
    }
}
