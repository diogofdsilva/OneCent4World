using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _1CW_Site.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }

        public IEnumerable<string > Tags { get; set; }
        public IEnumerable<int> TagsIds { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Address1")]
        public string Address1 { get; set; }

        [Display(Name = "Address2")]
        public string Address2 { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Country")]
        public string Country { get; set; }

        [Required]
        public int CountryId { get; set; }

        
        [Display(Name = "Image")]
        public byte[] Image { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = "URL")]
        public string Url { get; set; }
    }
}