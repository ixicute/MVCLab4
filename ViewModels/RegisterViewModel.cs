using Lab4.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lab4.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "First Name")]
        public string Firstname { get; set; }

        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Display(Name = "Username (Must be unique)")]
        public string Username { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Assign a role to the new user:")]
        public int roleType { get; set; } = 2;
        public List<Role>? roleList { get; set; }
    }
}
