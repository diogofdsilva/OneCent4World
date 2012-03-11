using System;
namespace OCW.DAL.DTOs
{
    public partial class Withdraw : Movement
    {
        #region Constructors
        protected Withdraw(){}
        #endregion
        #region Primitive Properties

        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual System.DateTime Date
        {
            get;
            set;
        }
    
        public virtual decimal Amount
        {
            get;
            set;
        }
    
        public virtual string Destination
        {
            get;
            set;
        }
    
        public virtual int Profile_id
        {
            get { return _profile_id; }
            set
            {
                if (_profile_id != value)
                {
                    if (Profile != null && Profile.Id != value)
                    {
                        Profile = null;
                    }
                    _profile_id = value;
                }
            }
        }
        private int _profile_id;

        #endregion
        #region Navigation Properties
    
        public virtual Profile Profile
        {
            get { return _profile; }
            set
            {
                if (!ReferenceEquals(_profile, value))
                {
                    var previousValue = _profile;
                    _profile = value;
                    FixupProfile(previousValue);
                }
            }
        }
        private Profile _profile;

        #endregion
        #region Association Fixup
    
        private void FixupProfile(Profile previousValue)
        {
            if (previousValue != null && previousValue.Withdraw.Contains(this))
            {
                previousValue.Withdraw.Remove(this);
            }
    
            if (Profile != null)
            {
                if (!Profile.Withdraw.Contains(this))
                {
                    Profile.Withdraw.Add(this);
                }
                if (Profile_id != Profile.Id)
                {
                    Profile_id = Profile.Id;
                }
            }
        }

        #endregion

        public override string TypeMovement
        {
            get { return "Withdraw"; }
        }

        public override DateTime DateMovement
        {
            get { return Date; }
        }

        public override string OrganizationMovement
        {
            get { return "-"; }
        }

        public override string SourceMovement
        {
            get { return "My Account"; }
        }

        public override string DestinyMovement
        {
            get { return Destination; }
        }

        public override decimal ValueMovement
        {
            get { return Amount; }
        }

        public override decimal DonationMovement
        {
            get { return 0; }
        }
    }
}
