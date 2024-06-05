using System.ComponentModel.DataAnnotations;

namespace ChefConnect.Models
{
    public class PaymentViewModel
    {
        [Required(ErrorMessage ="Please select Payment type")]
        public string PaymentType { get; set; }

        [Required(ErrorMessage = "Please enter a card number")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Please enter card expiry date")]
        public DateTime CardExpiry { get; set; }

        [Required(ErrorMessage = "Please enter card cvv")]
        public string CardCvv { get; set; }

        [Required(ErrorMessage = "Please enter name on card")]
        public string NameOnCard { get; set; }

        public string? Username { get; set; }

        public string? ReturnUrl { get; set; }
       
    }
}
