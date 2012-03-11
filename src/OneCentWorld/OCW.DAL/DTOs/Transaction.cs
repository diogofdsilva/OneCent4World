using System;

namespace OCW.DAL.DTOs
{
    public partial class Transaction : Movement
    {
        #region Constructors
        protected Transaction() { }
        #endregion
        #region Primitive Properties
    
        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual int Person_id
        {
            get { return _person_id; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_person_id != value)
                    {
                        if (Person != null && Person.Id != value)
                        {
                            Person = null;
                        }
                        _person_id = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private int _person_id;
    
        public virtual int Company_id
        {
            get { return _company_id; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_company_id != value)
                    {
                        if (Company != null && Company.Id != value)
                        {
                            Company = null;
                        }
                        _company_id = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private int _company_id;
    
        public virtual int? Organization_id
        {
            get { return _organization_id; }
            set
            {
                try
                {
                    _settingFK = true;
                    if (_organization_id != value)
                    {
                        if (Organization != null && Organization.Id != value)
                        {
                            Organization = null;
                        }
                        _organization_id = value;
                    }
                }
                finally
                {
                    _settingFK = false;
                }
            }
        }
        private int? _organization_id;
    
        public virtual decimal Value
        {
            get;
            set;
        }
    
        public virtual decimal Donation
        {
            get;
            set;
        }
    
        public virtual DateTime Date
        {
            get;
            set;
        }

        public virtual string Reference
        {
            get;
            set;
        }

        #endregion
        #region Navigation Properties
    
        public virtual Company Company
        {
            get { return _company; }
            set
            {
                if (!ReferenceEquals(_company, value))
                {
                    var previousValue = _company;
                    _company = value;
                    FixupCompany(previousValue);
                }
            }
        }
        private Company _company;
    
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
    
        public virtual Person Person
        {
            get { return _person; }
            set
            {
                if (!ReferenceEquals(_person, value))
                {
                    var previousValue = _person;
                    _person = value;
                    FixupPerson(previousValue);
                }
            }
        }
        private Person _person;

        #endregion
        #region Association Fixup
    
        private bool _settingFK = false;
    
        private void FixupCompany(Company previousValue)
        {
            if (previousValue != null && previousValue.Transaction.Contains(this))
            {
                previousValue.Transaction.Remove(this);
            }
    
            if (Company != null)
            {
                if (!Company.Transaction.Contains(this))
                {
                    Company.Transaction.Add(this);
                }
                if (Company_id != Company.Id)
                {
                    Company_id = Company.Id;
                }
            }
        }
    
        private void FixupOrganization(Organization previousValue)
        {
            if (previousValue != null && previousValue.Transaction.Contains(this))
            {
                previousValue.Transaction.Remove(this);
            }
    
            if (Organization != null)
            {
                if (!Organization.Transaction.Contains(this))
                {
                    Organization.Transaction.Add(this);
                }
                if (Organization_id != Organization.Id)
                {
                    Organization_id = Organization.Id;
                }
            }
            else if (!_settingFK)
            {
                Organization_id = null;
            }
        }
    
        private void FixupPerson(Person previousValue)
        {
            if (previousValue != null && previousValue.Transaction.Contains(this))
            {
                previousValue.Transaction.Remove(this);
            }
    
            if (Person != null)
            {
                if (!Person.Transaction.Contains(this))
                {
                    Person.Transaction.Add(this);
                }
                if (Person_id != Person.Id)
                {
                    Person_id = Person.Id;
                }
            }
        }

        #endregion


        public override string TypeMovement
        {
            get { return "Transaction"; }
        }

        public override DateTime DateMovement
        {
            get { return Date; }
        }

        public override string OrganizationMovement
        {
            get { return Organization.Name; }
        }

        public override string SourceMovement
        {
            get { return "My Account" ; }
        }

        public override string DestinyMovement
        {
            get { return Company.Name; }
        }

        public override decimal ValueMovement
        {
            get { return Value; }
        }

        public override decimal DonationMovement
        {
            get { return new decimal(0.01); }
        }
    }
}
