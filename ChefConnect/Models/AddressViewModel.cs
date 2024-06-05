using ChefConnect.Entities;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ChefConnect.Models
{
    public class AddressViewModel
    {
        [Required(ErrorMessage ="Please enter a Name")]
        public string Name { get; set; }

        public string? AptNumber { get; set; }

        [Required(ErrorMessage = "Please enter a Street Address")]
        public string StreetAddress { get; set; }

        [Required(ErrorMessage = "Please enter a City")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a Province")]
        public string Province { get; set; }

        [Required(ErrorMessage = "Please enter a Country")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Please enter a Postal Code")]
        [RegularExpression("^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$",ErrorMessage ="Please enter a valid postal code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Please enter a Phone Number")]
        [RegularExpression("^([2-9]{1}[0-9]{2})(([2-9]{1})(1[0,2-9]{1}|[0,2-9]{1}[0-9]{1}))([0-9]{4})$", ErrorMessage = "Please enter a valid phone number")]
        public string PhoneNumber { get; set; }

        public string? ReturnUrl { get; set; }

        public string? Username { get; set; }
        
    }
}
