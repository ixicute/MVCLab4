using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Availability
    {
        public int Id { get; set; }

        [Required]
        public string AvailabilityType { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
