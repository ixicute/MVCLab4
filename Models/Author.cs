using System.ComponentModel.DataAnnotations;

namespace Lab4.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public virtual ICollection<Book_Author_RT> BookAuthors { get; set; }
    }
}
