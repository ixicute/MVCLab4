using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lab4.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        [ForeignKey("Availabilities")]
        public int FK_AvailabilityId { get; set; }
        public virtual Availability Availabilities { get; set; }

        public virtual ICollection<Book_Author_RT> BookAuthors {get; set;}
        public virtual ICollection<Customer_Book_RT> CustomerBooks {get; set;}
    }
}
