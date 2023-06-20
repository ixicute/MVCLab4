using System.ComponentModel;

namespace Lab4.ViewModels
{
    public class BooksViewModel
    {
        public int BookId { get; set; }

        [DisplayName("Title")]
        public string BookTitle { get; set; }

        [DisplayName("Description")]
        public string BookDescription { get; set; }

        [DisplayName("Author/s")]
        public string BookAuthor { get; set; }

        public string BookImage { get; set; }

        [DisplayName("Status")]
        public string BookAvailability { get; set; }
    }
}
