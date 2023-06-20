using System.ComponentModel;

namespace Lab4.ViewModels
{
    public class BorrowViewModel
    {
        public int BookId { get; set; }

        [DisplayName("Title")]
        public string BookTitle { get; set; }

        [DisplayName("Author/s")]
        public string BookAuthor { get; set; }

        public string BookImage { get; set; }

    }
}
