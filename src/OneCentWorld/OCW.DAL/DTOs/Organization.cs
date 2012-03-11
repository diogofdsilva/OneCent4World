using System.Collections.Generic;
using System.Collections.Specialized;
using OCW.DAL.Visitor;

namespace OCW.DAL.DTOs
{
    public partial class Organization : Profile
    {
        #region Constructors
        protected Organization() { }
        #endregion
        #region Primitive Properties
    
        public virtual string Name
        {
            get;
            set;
        }
    
        public virtual byte[] Image
        {
            get;
            set;
        }
    
        public virtual string Url
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<Emergency> Emergency
        {
            get
            {
                if (_emergency == null)
                {
                    var newCollection = new FixupCollection<Emergency>();
                    newCollection.CollectionChanged += FixupEmergency;
                    _emergency = newCollection;
                }
                return _emergency;
            }
            set
            {
                if (!ReferenceEquals(_emergency, value))
                {
                    var previousValue = _emergency as FixupCollection<Emergency>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupEmergency;
                    }
                    _emergency = value;
                    var newValue = value as FixupCollection<Emergency>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupEmergency;
                    }
                }
            }
        }
        private ICollection<Emergency> _emergency;
    
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
    
        public virtual ICollection<Tag> Tag
        {
            get
            {
                if (_tag == null)
                {
                    var newCollection = new FixupCollection<Tag>();
                    newCollection.CollectionChanged += FixupTag;
                    _tag = newCollection;
                }
                return _tag;
            }
            set
            {
                if (!ReferenceEquals(_tag, value))
                {
                    var previousValue = _tag as FixupCollection<Tag>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupTag;
                    }
                    _tag = value;
                    var newValue = value as FixupCollection<Tag>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupTag;
                    }
                }
            }
        }
        private ICollection<Tag> _tag;

        #endregion
        #region Association Fixup
    
        private void FixupEmergency(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Emergency item in e.NewItems)
                {
                    item.Organization = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Emergency item in e.OldItems)
                {
                    if (ReferenceEquals(item.Organization, this))
                    {
                        item.Organization = null;
                    }
                }
            }
        }
    
        private void FixupTransaction(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Transaction item in e.NewItems)
                {
                    item.Organization = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Transaction item in e.OldItems)
                {
                    if (ReferenceEquals(item.Organization, this))
                    {
                        item.Organization = null;
                    }
                }
            }
        }
    
        private void FixupTag(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Tag item in e.NewItems)
                {
                    if (!item.Organization.Contains(this))
                    {
                        item.Organization.Add(this);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Tag item in e.OldItems)
                {
                    if (item.Organization.Contains(this))
                    {
                        item.Organization.Remove(this);
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
            get { return Name; }
        }
    }
}
