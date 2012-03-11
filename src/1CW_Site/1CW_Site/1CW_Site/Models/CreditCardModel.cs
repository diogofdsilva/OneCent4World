using System.ComponentModel.DataAnnotations;

namespace _1CW_Site.Models
{
    public class CreditCardModel
    {
        [Required]
        public string Issuer { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        [Display(Name ="Validity Month")]
        public int ValidityMonth { get; set; }

        [Required]
        [Display(Name = "Validity Year")]
        public int ValidityYear { get; set; }

        [Required]
        [Display(Name="CVV")]
        public int Cvv { get; set; }

        [Required]
        [Display(Name = "Name in the card")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Value { get; set;}
    }
}