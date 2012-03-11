using System.Collections.Generic;
using System.Collections.Specialized;

namespace OCW.DAL.DTOs
{
    public partial class Address
    {
        #region Constructors
        protected Address(){}
        #endregion
        #region Primitive Properties

        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual string Address1
        {
            get;
            set;
        }
    
        public virtual string Address2
        {
            get;
            set;
        }
    
        public virtual string PostalCode
        {
            get;
            set;
        }
    
        public virtual string City
        {
            get;
            set;
        }
    
        public virtual int Country_id
        {
            get { return _country_id; }
            set
            {
                if (_country_id != value)
                {
                    if (Country != null && Country.Id != value)
                    {
                        Country = null;
                    }
                    _country_id = value;
                }
            }
        }
        private int _country_id;

        #endregion
        #region Navigation Properties
    
        public virtual Country Country
        {
            get { return _country; }
            set
            {
                if (!ReferenceEquals(_country, value))
                {
                    var previousValue = _country;
                    _country = value;
                    FixupCountry(previousValue);
                }
            }
        }
        private Country _country;
    
        public virtual ICollection<Profile> Profile
        {
            get
            {
                if (_profile == null)
                {
                    var newCollection = new FixupCollection<Profile>();
                    newCollection.CollectionChanged += FixupProfile;
                    _profile = newCollection;
                }
                return _profile;
            }
            set
            {
                if (!ReferenceEquals(_profile, value))
                {
                    var previousValue = _profile as FixupCollection<Profile>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupProfile;
                    }
                    _profile = value;
                    var newValue = value as FixupCollection<Profile>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupProfile;
                    }
                }
            }
        }
        private ICollection<Profile> _profile;

        #endregion
        #region Association Fixup
    
        private void FixupCountry(Country previousValue)
        {
            if (previousValue != null && previousValue.Address.Contains(this))
            {
                previousValue.Address.Remove(this);
            }
    
            if (Country != null)
            {
                if (!Country.Address.Contains(this))
                {
                    Country.Address.Add(this);
                }
                if (Country_id != Country.Id)
                {
                    Country_id = Country.Id;
                }
            }
        }
    
        private void FixupProfile(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Profile item in e.NewItems)
                {
                    item.Address = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Profile item in e.OldItems)
                {
                    if (ReferenceEquals(item.Address, this))
                    {
                        item.Address = null;
                    }
                }
            }
        }

        #endregion
    }
}
