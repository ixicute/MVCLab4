using Lab4.Models;
using System.ComponentModel;

namespace Lab4.ViewModels
{
    public class UserViewModel
    {
        [DisplayName("ID")]
        public int UserId { get; set; }

        [DisplayName("First Name")]
        public string UserFirstName { get; set; }

        [DisplayName("Last Name")]
        public string UserLastName { get; set; }

        [DisplayName("Username")]
        public string UserName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }

        public int RoleId { get; set; }

        public string? Role { get; set; }

        public List<Role>? RoleList { get; set; }

        public string Email { get; set; }
    }
}
