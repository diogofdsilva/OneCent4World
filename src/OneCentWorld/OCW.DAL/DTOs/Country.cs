using System.Collections.Generic;
using System.Collections.Specialized;

namespace OCW.DAL.DTOs
{
    public partial class Country
    {
        #region Constructors
        protected Country() { }
        #endregion
        #region Primitive Properties
    
        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual string Name
        {
            get;
            set;
        }
    
        public virtual string Alpha2
        {
            get;
            set;
        }
    
        public virtual string Alpha3
        {
            get;
            set;
        }
    
        public virtual string Numeric
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<Address> Address
        {
            get
            {
                if (_address == null)
                {
                    var newCollection = new FixupCollection<Address>();
                    newCollection.CollectionChanged += FixupAddress;
                    _address = newCollection;
                }
                return _address;
            }
            set
            {
                if (!ReferenceEquals(_address, value))
                {
                    var previousValue = _address as FixupCollection<Address>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupAddress;
                    }
                    _address = value;
                    var newValue = value as FixupCollection<Address>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupAddress;
                    }
                }
            }
        }
        private ICollection<Address> _address;

        #endregion
        #region Association Fixup
    
        private void FixupAddress(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Address item in e.NewItems)
                {
                    item.Country = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Address item in e.OldItems)
                {
                    if (ReferenceEquals(item.Country, this))
                    {
                        item.Country = null;
                    }
                }
            }
        }

        #endregion
    }
}
