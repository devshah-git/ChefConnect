using System;
using System.ComponentModel.DataAnnotations;

namespace ChefConnect.Models
{
	public class RegisterChefViewModel
	{
        [Required(ErrorMessage ="Please enter valid username.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage ="Please enter a valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your Full Name here.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please enter your password here.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please again enter your password here.")]
        [DataType(DataType.Password)]
        [Compare("Password")]
		public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter a valid Canadian Phone Number")]
        [StringLength(10,ErrorMessage ="Please enter a vaild Canadian Phone Number")]
		public string PhoneNumber { get; set; }
	}
}

