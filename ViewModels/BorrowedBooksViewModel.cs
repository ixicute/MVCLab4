using System.ComponentModel;

namespace Lab4.ViewModels
{
    public class BorrowedBooksViewModel
    {
        public int BookId { get; set; }
        public int CBRTId { get; set; }

        [DisplayName("Title")]
        public string BookTitle { get; set; }

        [DisplayName("Description")]
        public string BookDescription { get; set; }

        [DisplayName("Author/s")]
        public string BookAuthor { get; set; }

        public string BookImage { get; set; }

        public bool? isReturned { get; set; }

        public DateTime BorrowedDate { get; set; }

        public DateTime ReturnAt { get; set; }
    }
}
