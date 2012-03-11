using System.Collections.Generic;
using System.Collections.Specialized;
using OCW.DAL.Visitor;

namespace OCW.DAL.DTOs
{
    public partial class Company : Profile
    {
        #region Constructors
        protected Company() { }
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

        private void FixupTag(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Tag item in e.NewItems)
                {
                    if (!item.Company.Contains(this))
                    {
                        item.Company.Add(this);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Tag item in e.OldItems)
                {
                    if (item.Company.Contains(this))
                    {
                        item.Company.Remove(this);
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
                    item.Company = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Transaction item in e.OldItems)
                {
                    if (ReferenceEquals(item.Company, this))
                    {
                        item.Company = null;
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
