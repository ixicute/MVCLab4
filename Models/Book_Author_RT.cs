using System.ComponentModel.DataAnnotations.Schema;

namespace Lab4.Models
{
    public class Book_Author_RT
    {
        public int Id { get; set; }

        [ForeignKey("Books")]
        public int FK_BookId { get; set; }
        public virtual Book Books { get; set; }

        [ForeignKey("Authors")]
        public int FK_AuthorId { get; set; }
        public virtual Author Authors { get; set; }
    }
}
