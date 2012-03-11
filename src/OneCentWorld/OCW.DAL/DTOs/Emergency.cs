using System;

namespace OCW.DAL.DTOs
{
    public partial class Emergency
    {
        #region Constructors
        protected Emergency() { }
        #endregion
        #region Primitive Properties
    
        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual string Title
        {
            get;
            set;
        }
    
        public virtual string Description
        {
            get;
            set;
        }
    
        public virtual System.DateTime StartDate
        {
            get;
            set;
        }
    
        public virtual decimal Weight
        {
            get;
            set;
        }
    
        public virtual Nullable<System.DateTime> EndDate
        {
            get;
            set;
        }
    
        public virtual byte[] Image
        {
            get;
            set;
        }
    
        public virtual int Organization_id
        {
            get { return _organization_id; }
            set
            {
                if (_organization_id != value)
                {
                    if (Organization != null && Organization.Id != value)
                    {
                        Organization = null;
                    }
                    _organization_id = value;
                }
            }
        }
        private int _organization_id;

        #endregion
        #region Navigation Properties
    
        public virtual Organization Organization
        {
            get { return _organization; }
            set
            {
                if (!ReferenceEquals(_organization, value))
                {
                    var previousValue = _organization;
                    _organization = value;
                    FixupOrganization(previousValue);
                }
            }
        }
        private Organization _organization;

        #endregion
        #region Association Fixup
    
        private void FixupOrganization(Organization previousValue)
        {
            if (previousValue != null && previousValue.Emergency.Contains(this))
            {
                previousValue.Emergency.Remove(this);
            }
    
            if (Organization != null)
            {
                if (!Organization.Emergency.Contains(this))
                {
                    Organization.Emergency.Add(this);
                }
                if (Organization_id != Organization.Id)
                {
                    Organization_id = Organization.Id;
                }
            }
        }

        #endregion
    }
}
