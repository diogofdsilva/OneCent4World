using System.Collections.Generic;
using System.Collections.Specialized;

namespace OCW.DAL.DTOs
{
    public partial class Tag
    {
        #region Constructors
        protected Tag() { }
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

        #endregion
        #region Navigation Properties
    
        public virtual ICollection<Organization> Organization
        {
            get
            {
                if (_organization == null)
                {
                    var newCollection = new FixupCollection<Organization>();
                    newCollection.CollectionChanged += FixupOrganization;
                    _organization = newCollection;
                }
                return _organization;
            }
            set
            {
                if (!ReferenceEquals(_organization, value))
                {
                    var previousValue = _organization as FixupCollection<Organization>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupOrganization;
                    }
                    _organization = value;
                    var newValue = value as FixupCollection<Organization>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupOrganization;
                    }
                }
            }
        }
        private ICollection<Organization> _organization;
    
        public virtual ICollection<Company> Company
        {
            get
            {
                if (_company == null)
                {
                    var newCollection = new FixupCollection<Company>();
                    newCollection.CollectionChanged += FixupCompany;
                    _company = newCollection;
                }
                return _company;
            }
            set
            {
                if (!ReferenceEquals(_company, value))
                {
                    var previousValue = _company as FixupCollection<Company>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupCompany;
                    }
                    _company = value;
                    var newValue = value as FixupCollection<Company>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupCompany;
                    }
                }
            }
        }
        private ICollection<Company> _company;

        #endregion
        #region Association Fixup
    
        private void FixupOrganization(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Organization item in e.NewItems)
                {
                    if (!item.Tag.Contains(this))
                    {
                        item.Tag.Add(this);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Organization item in e.OldItems)
                {
                    if (item.Tag.Contains(this))
                    {
                        item.Tag.Remove(this);
                    }
                }
            }
        }
    
        private void FixupCompany(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Company item in e.NewItems)
                {
                    if (!item.Tag.Contains(this))
                    {
                        item.Tag.Add(this);
                    }
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Company item in e.OldItems)
                {
                    if (item.Tag.Contains(this))
                    {
                        item.Tag.Remove(this);
                    }
                }
            }
        }

        #endregion
    }
}
