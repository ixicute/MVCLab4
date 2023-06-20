using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Lab4.Models
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Address")]
        [Required]
        public string Street { get; set; }

        [DisplayName("Ort")]
        [Required]
        public string City { get; set; }

        [DisplayName("postnr")]
        [Required]
        public string PostalCode { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
