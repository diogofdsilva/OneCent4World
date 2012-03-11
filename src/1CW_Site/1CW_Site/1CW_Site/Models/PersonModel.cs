using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _1CW_Site.Models
{

    public class PersonModel
    {
        
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisteredDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set;}

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        public int CountryId { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class DepositModel
    {
        public int Id { get; set; }

        [Display(Name="Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name= "Value")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        [Display(Name = "From")]
        [DataType(DataType.Text)]
        public String Source { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            DepositModel model = obj as DepositModel;
            if (model == null) return false;
            return model.Id == Id && model.Date == Date && model.Value == Value && model.Source == Source;
        }

        public bool Equals(DepositModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && other.Date.Equals(Date) && other.Value == Value && Equals(other.Source, Source);
        }

        public static bool operator ==(DepositModel model1, DepositModel model2)
        {
            return !ReferenceEquals(model1, null) ? model1.Equals(model2) : ReferenceEquals(model2, null);
        }

        public static bool operator !=(DepositModel model1, DepositModel model2)
        {
            return !(model1 == model2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result*397) ^ Date.GetHashCode();
                result = (result*397) ^ Value.GetHashCode();
                result = (result*397) ^ (Source != null ? Source.GetHashCode() : 0);
                return result;
            }
        }
    }

    public class WithdrawModel
    {
        public int Id { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Value")]
        [DataType(DataType.Currency)]
        public decimal Value { get; set; }

        [Display(Name = "To")]
        [DataType(DataType.Text)]
        public String Destination { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            WithdrawModel model = obj as WithdrawModel;
            if (model == null) return false;
            return model.Id == Id && model.Date == Date && model.Value == Value && model.Destination == Destination;
        }


        public bool Equals(WithdrawModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Id == Id && other.Date.Equals(Date) && other.Value == Value && Equals(other.Destination, Destination);
        }

        public static bool operator ==(WithdrawModel model1, WithdrawModel model2)
        {
            return !ReferenceEquals(model1, null) ? model1.Equals(model2) : ReferenceEquals(model2, null);
        }

        public static bool operator !=(WithdrawModel model1, WithdrawModel model2)
        {
            return !(model1 == model2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Id;
                result = (result*397) ^ Date.GetHashCode();
                result = (result*397) ^ Value.GetHashCode();
                result = (result*397) ^ (Destination != null ? Destination.GetHashCode() : 0);
                return result;
            }
        }
    }
}