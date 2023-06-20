using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Net;

namespace Lab4.Models
{
    public class User : IdentityUser<int>
    {
        [DisplayName("Förnamn")]
        [Required]
        public string FirstName { get; set; }

        [DisplayName("Efternamn")]
        [Required]
        public string LastName { get; set; }

        [ForeignKey("Addresses")]
        public int FK_AddressId { get; set; }
        public virtual Address Addresses { get; set; }

        [ForeignKey("Roles")]
        public int FK_RoleId { get; set; }
        public virtual Role Roles { get; set; }

        public virtual ICollection<Customer_Book_RT> CustomerBooks { get; set; }
    }
}
