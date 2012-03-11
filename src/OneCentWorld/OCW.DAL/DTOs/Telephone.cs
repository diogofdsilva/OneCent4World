namespace OCW.DAL.DTOs
{
    public partial class Telephone
    {
        #region Constructors
        protected Telephone() { }
        #endregion
        #region Primitive Properties
    
        public virtual int Id
        {
            get;
            set;
        }
    
        public virtual string Telephone1
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
            if (previousValue != null && previousValue.Telephone.Contains(this))
            {
                previousValue.Telephone.Remove(this);
            }
    
            if (Profile != null)
            {
                if (!Profile.Telephone.Contains(this))
                {
                    Profile.Telephone.Add(this);
                }
                if (Profile_id != Profile.Id)
                {
                    Profile_id = Profile.Id;
                }
            }
        }

        #endregion
    }
}
