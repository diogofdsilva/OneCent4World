using System.Collections.Generic;
using System.Collections.Specialized;

namespace OCW.DAL.DTOs
{
    public partial class Account
    {
        #region Constructors
        protected Account(){}
        #endregion
        #region Primitive Properties

        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual decimal Value
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
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
    
        private void FixupProfile(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Profile item in e.NewItems)
                {
                    item.Account = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Profile item in e.OldItems)
                {
                    if (ReferenceEquals(item.Account, this))
                    {
                        item.Account = null;
                    }
                }
            }
        }

        #endregion
    }
}
