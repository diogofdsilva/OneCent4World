using System.Collections.Generic;
using System.Collections.Specialized;
using OCW.DAL.Visitor;

namespace OCW.DAL.DTOs
{
    public abstract partial class Profile
    {
        #region Constructors
        protected Profile() { }
        #endregion
        #region Primitive Properties
    
        public virtual int Id
        {
            get;
            set;
        }

        public abstract string ProfileName { get; }

        public virtual string Email
        {
            get;
            set;
        }
    
        public virtual string Password
        {
            get;
            set;
        }
    
        public virtual System.DateTime RegisteredDate
        {
            get;
            set;
        }
    
        public virtual int Address_id
        {
            get { return _address_id; }
            set
            {
                if (_address_id != value)
                {
                    if (Address != null && Address.Id != value)
                    {
                        Address = null;
                    }
                    _address_id = value;
                }
            }
        }
        private int _address_id;
    
        public virtual int Account_id
        {
            get { return _account_id; }
            set
            {
                if (_account_id != value)
                {
                    if (Account != null && Account.Id != value)
                    {
                        Account = null;
                    }
                    _account_id = value;
                }
            }
        }
        private int _account_id;

        #endregion
        #region Navigation Properties
    
        public virtual Account Account
        {
            get { return _account; }
            set
            {
                if (!ReferenceEquals(_account, value))
                {
                    var previousValue = _account;
                    _account = value;
                    FixupAccount(previousValue);
                }
            }
        }
        private Account _account;
    
        public virtual Address Address
        {
            get { return _address; }
            set
            {
                if (!ReferenceEquals(_address, value))
                {
                    var previousValue = _address;
                    _address = value;
                    FixupAddress(previousValue);
                }
            }
        }
        private Address _address;
    
        public virtual ICollection<Telephone> Telephone
        {
            get
            {
                if (_telephone == null)
                {
                    var newCollection = new FixupCollection<Telephone>();
                    newCollection.CollectionChanged += FixupTelephone;
                    _telephone = newCollection;
                }
                return _telephone;
            }
            set
            {
                if (!ReferenceEquals(_telephone, value))
                {
                    var previousValue = _telephone as FixupCollection<Telephone>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupTelephone;
                    }
                    _telephone = value;
                    var newValue = value as FixupCollection<Telephone>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupTelephone;
                    }
                }
            }
        }
        private ICollection<Telephone> _telephone;
    
        public virtual ICollection<Deposit> Deposit
        {
            get
            {
                if (_deposit == null)
                {
                    var newCollection = new FixupCollection<Deposit>();
                    newCollection.CollectionChanged += FixupDeposit;
                    _deposit = newCollection;
                }
                return _deposit;
            }
            set
            {
                if (!ReferenceEquals(_deposit, value))
                {
                    var previousValue = _deposit as FixupCollection<Deposit>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupDeposit;
                    }
                    _deposit = value;
                    var newValue = value as FixupCollection<Deposit>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupDeposit;
                    }
                }
            }
        }
        private ICollection<Deposit> _deposit;
    
        public virtual ICollection<Withdraw> Withdraw
        {
            get
            {
                if (_withdraw == null)
                {
                    var newCollection = new FixupCollection<Withdraw>();
                    newCollection.CollectionChanged += FixupWithdraw;
                    _withdraw = newCollection;
                }
                return _withdraw;
            }
            set
            {
                if (!ReferenceEquals(_withdraw, value))
                {
                    var previousValue = _withdraw as FixupCollection<Withdraw>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupWithdraw;
                    }
                    _withdraw = value;
                    var newValue = value as FixupCollection<Withdraw>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupWithdraw;
                    }
                }
            }
        }
        private ICollection<Withdraw> _withdraw;

        #endregion
        #region Association Fixup
    
        private void FixupAccount(Account previousValue)
        {
            if (previousValue != null && previousValue.Profile.Contains(this))
            {
                previousValue.Profile.Remove(this);
            }
    
            if (Account != null)
            {
                if (!Account.Profile.Contains(this))
                {
                    Account.Profile.Add(this);
                }
                if (Account_id != Account.Id)
                {
                    Account_id = Account.Id;
                }
            }
        }
    
        private void FixupAddress(Address previousValue)
        {
            if (previousValue != null && previousValue.Profile.Contains(this))
            {
                previousValue.Profile.Remove(this);
            }
    
            if (Address != null)
            {
                if (!Address.Profile.Contains(this))
                {
                    Address.Profile.Add(this);
                }
                if (Address_id != Address.Id)
                {
                    Address_id = Address.Id;
                }
            }
        }
    
        private void FixupTelephone(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Telephone item in e.NewItems)
                {
                    item.Profile = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Telephone item in e.OldItems)
                {
                    if (ReferenceEquals(item.Profile, this))
                    {
                        item.Profile = null;
                    }
                }
            }
        }
    
        private void FixupDeposit(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Deposit item in e.NewItems)
                {
                    item.Profile = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Deposit item in e.OldItems)
                {
                    if (ReferenceEquals(item.Profile, this))
                    {
                        item.Profile = null;
                    }
                }
            }
        }
    
        private void FixupWithdraw(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Withdraw item in e.NewItems)
                {
                    item.Profile = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Withdraw item in e.OldItems)
                {
                    if (ReferenceEquals(item.Profile, this))
                    {
                        item.Profile = null;
                    }
                }
            }
        }

        #endregion
        #region Visitor
        public virtual void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        #endregion
    }
}
