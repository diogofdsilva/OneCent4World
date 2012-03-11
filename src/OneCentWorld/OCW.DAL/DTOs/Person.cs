using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OCW.DAL.Visitor;

namespace OCW.DAL.DTOs
{
    public partial class Person : Profile
    {
        #region Constructors
        protected Person() { }
        #endregion
        #region Primitive Properties
    
        public virtual string FirstName
        {
            get;
            set;
        }
    
        public virtual string LastName
        {
            get;
            set;
        }
    
        public virtual DateTime? Birthdate
        {
            get;
            set;
        }
    
        public virtual int Role_id
        {
            get { return _role_id; }
            set
            {
                if (_role_id != value)
                {
                    if (Role != null && Role.Id != value)
                    {
                        Role = null;
                    }
                    _role_id = value;
                }
            }
        }
        private int _role_id;

        #endregion
        #region Navigation Properties
    
        public virtual Role Role
        {
            get { return _role; }
            set
            {
                if (!ReferenceEquals(_role, value))
                {
                    var previousValue = _role;
                    _role = value;
                    FixupRole(previousValue);
                }
            }
        }
        private Role _role;
    
        public virtual ICollection<Transaction> Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    var newCollection = new FixupCollection<Transaction>();
                    newCollection.CollectionChanged += FixupTransaction;
                    _transaction = newCollection;
                }
                return _transaction;
            }
            set
            {
                if (!ReferenceEquals(_transaction, value))
                {
                    var previousValue = _transaction as FixupCollection<Transaction>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupTransaction;
                    }
                    _transaction = value;
                    var newValue = value as FixupCollection<Transaction>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupTransaction;
                    }
                }
            }
        }
        private ICollection<Transaction> _transaction;

        #endregion
        #region Association Fixup
    
        private void FixupRole(Role previousValue)
        {
            if (previousValue != null && previousValue.Person.Contains(this))
            {
                previousValue.Person.Remove(this);
            }
    
            if (Role != null)
            {
                if (!Role.Person.Contains(this))
                {
                    Role.Person.Add(this);
                }
                if (Role_id != Role.Id)
                {
                    Role_id = Role.Id;
                }
            }
        }
    
        private void FixupTransaction(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Transaction item in e.NewItems)
                {
                    item.Person = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Transaction item in e.OldItems)
                {
                    if (ReferenceEquals(item.Person, this))
                    {
                        item.Person = null;
                    }
                }
            }
        }

        #endregion
        #region Visitor
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
        #endregion
        public override string ProfileName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}
