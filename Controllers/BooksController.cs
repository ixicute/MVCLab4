using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.ViewModels;
using Lab4.CustomIdentity;
using Lab4.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Lab4.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly CustomUserManager userManager;

        public BooksController(ApplicationDbContext _context, CustomUserManager _userManager)
        {
            userManager = _userManager;
            context = _context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allBooks = await (from baRT in context.BookAuthorsRT
                                  join b in context.Books on baRT.FK_BookId equals b.Id
                                  join a in context.Authors on baRT.FK_AuthorId equals a.Id
                                  join av in context.Availabilities on b.FK_AvailabilityId equals av.Id
                                  select new
                                  {
                                      BookId = b.Id,
                                      BookTitle = b.Title,
                                      BookDescription = b.Description,
                                      BookImage = b.Image,
                                      BookAvailability = av.AvailabilityType,
                                      BookAuthor = a.FirstName + " " + a.LastName
                                  })
                                  .GroupBy(b => new
                                  {
                                      b.BookId,
                                      b.BookTitle,
                                      b.BookDescription,
                                      b.BookImage,
                                      b.BookAvailability
                                  })
                                  .Select(group => new
                                  {
                                      group.Key.BookId,
                                      group.Key.BookTitle,
                                      group.Key.BookDescription,
                                      group.Key.BookImage,
                                      group.Key.BookAvailability,
                                      BookAuthors = string.Join(", ", group.Select(a => a.BookAuthor))
                                  })
                                  .ToListAsync();

            var booksList = new List<BooksViewModel>();

            foreach(var book in allBooks)
            {
                var libraryBook = new BooksViewModel();
                libraryBook.BookTitle = book.BookTitle;
                libraryBook.BookDescription = book.BookDescription;
                libraryBook.BookAuthor = book.BookAuthors;
                libraryBook.BookAvailability = book.BookAvailability;
                libraryBook.BookImage = book.BookImage;
                libraryBook.BookId = book.BookId;

                booksList.Add(libraryBook);
            }

            return View(booksList);
        }

        [Authorize]
        public async Task<IActionResult> Borrow(int bookId = 1)
        {
            var book = await (from baRT in context.BookAuthorsRT
                                  join b in context.Books on baRT.FK_BookId equals b.Id
                                  join a in context.Authors on baRT.FK_AuthorId equals a.Id
                                  join av in context.Availabilities on b.FK_AvailabilityId equals av.Id
                                  where b.Id == bookId
                                  select new
                                  {
                                      BookId = b.Id,
                                      BookTitle = b.Title,
                                      BookImage = b.Image,
                                      BookAuthor = a.FirstName + " " + a.LastName
                                  })
                                  .GroupBy(b => new
                                  {
                                      b.BookId,
                                      b.BookTitle,
                                      b.BookImage
                                  })
                                  .Select(group => new
                                  {
                                      group.Key.BookId,
                                      group.Key.BookTitle,
                                      group.Key.BookImage,
                                      BookAuthors = string.Join(", ", group.Select(a => a.BookAuthor))
                                  })
                                  .FirstOrDefaultAsync();

            var toBorrow = new BorrowViewModel()
            {
                BookId = book.BookId,
                BookTitle = book.BookTitle,
                BookAuthor = book.BookAuthors,
                BookImage = book.BookImage
            };

            return View(toBorrow);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SubmitBorrow(int bookId)
        {
            var currentUser = userManager.GetUserAsync(User).Result;

            var today = DateTime.Now.Date;

            var newBorrow = new Customer_Book_RT()
            {
                FK_BookId = bookId,
                FK_CustomerId = currentUser.Id,
                BorrowDate = today,
                ReturnDate = today.AddDays(7),
            };
            context.CustomerBooksRT.Add(newBorrow);
            var check = await context.SaveChangesAsync();
            if(check != 0)
            {
                TempData["Confirm"] = "The book has been successfully added to your library!";
                return RedirectToAction("Confirmation", "Home");
            }

            TempData["Confirm"] = "An error occured. Try again later.";
            return RedirectToAction("Confirmation", "Home");
        }
        
        [Authorize]
        public async Task<IActionResult> MyBooks(bool allBorrowedBooks = false)
        {

            var currentUser = userManager.GetUserAsync(User).Result;

            var userBooks = await (from cbRT in context.CustomerBooksRT
                                    join b in context.Books on cbRT.FK_BookId equals b.Id
                                    join u in context.Users on cbRT.FK_CustomerId equals u.Id
                                    join baRT in context.BookAuthorsRT on b.Id equals baRT.FK_BookId
                                    join a in context.Authors on baRT.FK_AuthorId equals a.Id
                                    where cbRT.FK_CustomerId == currentUser.Id && (allBorrowedBooks || cbRT.IsReturned != true)
                                    select new
                                    {
                                        BookId = b.Id,
                                        cbRTID = cbRT.Id,
                                        IsReturned = cbRT.IsReturned,
                                        BookTitle = b.Title,
                                        BookDescription = b.Description,
                                        BookImage = b.Image,
                                        BorrowDate = cbRT.BorrowDate,
                                        ReturnAt = cbRT.ReturnDate,
                                        BookAuthor = a.FirstName + " " + a.LastName
                                    })
                                  .GroupBy(b => new
                                  {
                                      b.BookId,
                                      b.cbRTID,
                                      b.IsReturned,
                                      b.BookTitle,
                                      b.BookDescription,
                                      b.BorrowDate,
                                      b.ReturnAt,
                                      b.BookImage
                                  })
                                  .Select(group => new
                                  {
                                      group.Key.BookId,
                                      group.Key.cbRTID,
                                      group.Key.IsReturned,
                                      group.Key.BookTitle,
                                      group.Key.BookDescription,
                                      group.Key.BorrowDate,
                                      group.Key.ReturnAt,
                                      group.Key.BookImage,
                                      BookAuthors = string.Join(", ", group.Select(a => a.BookAuthor))
                                  }).ToListAsync();

            var userBorrowedList = new List<BorrowedBooksViewModel>();

            foreach (var book in userBooks)
            {
                var borrowedBook = new BorrowedBooksViewModel();
                borrowedBook.BookTitle = book.BookTitle;
                borrowedBook.CBRTId = book.cbRTID;
                borrowedBook.isReturned = book.IsReturned;
                borrowedBook.BookDescription = book.BookDescription;
                borrowedBook.BookAuthor = book.BookAuthors;
                borrowedBook.BookImage = book.BookImage;
                borrowedBook.BookId = book.BookId;
                borrowedBook.BorrowedDate = book.BorrowDate;
                borrowedBook.ReturnAt = book.ReturnAt;

                userBorrowedList.Add(borrowedBook);
            }

            return View(userBorrowedList);
        }

        [Authorize]
        public async Task<IActionResult> ReturnBook(int cbRTID)
        {
            var bookToReturn = await context.CustomerBooksRT.FirstOrDefaultAsync(cbRT => cbRT.Id == cbRTID);

            if (bookToReturn.ReturnDate.Date !> DateTime.Today.Date)
            {
                bookToReturn.IsReturnedLate = false;

            } else
            {
                bookToReturn.IsReturnedLate = true;

            }
            bookToReturn.ReturnDate = DateTime.Now.Date;

            bookToReturn.IsReturned = true;

            await context.SaveChangesAsync();

            //redirect to MyBooks with confirmation text.
            return RedirectToAction("MyBooks", "Books");
        }
    }
}
