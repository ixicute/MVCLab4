using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Customer_Book_RT
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Books")]
        public int FK_BookId { get; set; }
        public virtual Book Books { get; set; }

        [Required]
        [ForeignKey("Users")]
        public int FK_CustomerId { get; set; }
        public virtual User Users { get; set; }

        [Required]
        public DateTime BorrowDate { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; }

        public bool? IsReturned { get; set; } = false;

        public bool? IsReturnedLate { get; set; } = false;
    }
}
