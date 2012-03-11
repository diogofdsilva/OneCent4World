using System;
using System.ComponentModel.DataAnnotations;

namespace _1CW_Site.Models
{
    public class TransactionModel
    {
        public string Reference { get; set; }

        public bool OrganizationHasImage { get; set; }

        public bool CompanyHasImage { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Organization")]
        public string OrganizationName { get; set; }

        [Required]
        public int OrganizationId { get; set; }

        //[Required]
        [DataType(DataType.Text)]
        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        //[Required]
        public int CompanyId { get; set; }

        //[Required]
        [DataType(DataType.Text)]
        [Display(Name = "Person")]
        public string PersonName { get; set; }

        //[Required]
        [Display(Name = "Paid")]
        [DataType(DataType.Currency)]
        public decimal PaidAmount { get; set; }

        //[Required]
        [Display(Name = "Donation")]
        //[DataType(DataType.Currency)]
        public decimal DonationAmount { get; set; }
    }
}