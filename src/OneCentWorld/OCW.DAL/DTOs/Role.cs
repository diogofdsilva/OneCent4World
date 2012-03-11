using System.Collections.Generic;
using System.Collections.Specialized;

namespace OCW.DAL.DTOs
{
    public partial class Role
    {
        #region Constructors
        protected Role() { }
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
    
        public virtual ICollection<Person> Person
        {
            get
            {
                if (_person == null)
                {
                    var newCollection = new FixupCollection<Person>();
                    newCollection.CollectionChanged += FixupPerson;
                    _person = newCollection;
                }
                return _person;
            }
            set
            {
                if (!ReferenceEquals(_person, value))
                {
                    var previousValue = _person as FixupCollection<Person>;
                    if (previousValue != null)
                    {
                        previousValue.CollectionChanged -= FixupPerson;
                    }
                    _person = value;
                    var newValue = value as FixupCollection<Person>;
                    if (newValue != null)
                    {
                        newValue.CollectionChanged += FixupPerson;
                    }
                }
            }
        }
        private ICollection<Person> _person;

        #endregion
        #region Association Fixup
    
        private void FixupPerson(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Person item in e.NewItems)
                {
                    item.Role = this;
                }
            }
    
            if (e.OldItems != null)
            {
                foreach (Person item in e.OldItems)
                {
                    if (ReferenceEquals(item.Role, this))
                    {
                        item.Role = null;
                    }
                }
            }
        }

        #endregion
    }
}
