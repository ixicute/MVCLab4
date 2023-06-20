using Lab4.CustomIdentity;
using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Lab4.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly CustomUserManager userManager;
        private readonly SignInManager<User> signInManager;
        public UserController(ApplicationDbContext _context, CustomUserManager _userManager, SignInManager<User> _signInManager)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        //Get all users in database.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var usersList = new List<UserDataViewModel>();
            var allUsers = await context.Users.ToListAsync();
            foreach(var user in allUsers)
            {
                var userData = new UserDataViewModel()
                {
                    FullName = user.FirstName + " " + user.LastName,
                    Email = user.Email,
                    Id = user.Id,
                    UserName = user.UserName
                };

                usersList.Add(userData);
            }

            return View(usersList);
        }

        //When selecting a user, see all of the books they have borrowed.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserBooksById(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            var userBooks = await (from cbRT in context.CustomerBooksRT
                                   join b in context.Books on cbRT.FK_BookId equals b.Id
                                   join u in context.Users on cbRT.FK_CustomerId equals u.Id
                                   join baRT in context.BookAuthorsRT on b.Id equals baRT.FK_BookId
                                   join a in context.Authors on baRT.FK_AuthorId equals a.Id
                                   where cbRT.FK_CustomerId == user.Id
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersById(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == user.FK_AddressId);
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == user.FK_RoleId);

            var pickedUser = new UserViewModel()
            {
                UserId = user.Id,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserName = user.UserName,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                Role = userRole.Name,
                Email = user.Email
            };

            return View(pickedUser);
        }

        //Edit customer data
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == user.FK_AddressId);
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == user.FK_RoleId);

            var pickedUser = new UserViewModel()
            {
                UserId = user.Id,
                UserFirstName = user.FirstName,
                UserLastName = user.LastName,
                UserName = user.UserName,
                Street = address.Street,
                City = address.City,
                PostalCode = address.PostalCode,
                RoleId = userRole.Id,
                Role = userRole.Name,
                Email = user.Email
            };

            pickedUser.RoleList = new List<Role>();
            var allAvailableRoles = await context.Roles.ToListAsync();
            pickedUser.RoleList.AddRange(allAvailableRoles);

            return View(pickedUser);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(UserViewModel editedUserData)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == editedUserData.UserId);
            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == user.FK_AddressId);
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Id == user.FK_RoleId);

            if (ModelState.IsValid)
            {
                user.FirstName = editedUserData.UserFirstName;
                user.LastName = editedUserData.UserLastName;
                user.UserName = editedUserData.UserName;
                user.Email = editedUserData.Email;
                if (editedUserData.Street != address.Street && !string.IsNullOrEmpty(editedUserData.Street))
                {
                    address.Street = editedUserData.Street;
                }
                if (editedUserData.City != address.City && !string.IsNullOrEmpty(editedUserData.City))
                {
                    address.City = editedUserData.City;
                }
                if (editedUserData.PostalCode != address.PostalCode && !string.IsNullOrEmpty(editedUserData.PostalCode))
                {
                    address.PostalCode = editedUserData.PostalCode;
                }
                if (editedUserData.RoleId != userRole.Id)
                {
                    user.FK_RoleId = editedUserData.RoleId;
                }

                await context.SaveChangesAsync();
            }

            TempData["Confirm"] = "The user has been updated successfully!";
            return RedirectToAction("Confirmation", "Home");
        }

        //Create a new customer
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewUser()
        {
            List<Role> roleList = new List<Role>();
            roleList = await context.Roles.ToListAsync();
            RegisterViewModel response = new RegisterViewModel();

            response.roleType = 0;
            response.roleList = new List<Role>();
            response.roleList.AddRange(roleList);

            return View(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewUser(RegisterViewModel newUser)
        {
            if (!ModelState.IsValid)
            {
                TempData["Invalid"] = "Invalid properties. Please make sure all fields are filld!";
                return RedirectToAction("CreateUser", "Account");
            }

            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user != null)
            {
                TempData["Email"] = "This email already exists. Try again.";
                return RedirectToAction("CreateUser", "Account");
            }

            var username = await context.Users.FirstOrDefaultAsync(u => u.UserName == newUser.Username);
            if (username != null)
            {
                TempData["Username"] = "This username is already taken. Try again.";
                return RedirectToAction("CreateUser", "Account");
            }

            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Street.ToLower() == newUser.Street.ToLower() && a.City.ToLower() == newUser.City.ToLower());
            if (address == null)
            {
                address = new Address
                {
                    Street = newUser.Street,
                    City = newUser.City,
                    PostalCode = newUser.PostalCode
                };
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
            }

            var addUser = new User()
            {
                FirstName = newUser.Firstname,
                LastName = newUser.Lastname,
                FK_AddressId = address.Id,
                FK_RoleId = newUser.roleType,
                UserName = newUser.Username,
                Email = newUser.Email,
                EmailConfirmed = true
            };

            var newUserResponse = await userManager.CreateAsync(addUser, newUser.Password);

            if (newUserResponse.Succeeded)
            {
                //should redirect to confirmation page and send the "TempData" to it.
                TempData["Confirm"] = "The user has been created successfully!";
                return RedirectToAction("Confirmation", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        //Remove customer
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            TempData["Confirm"] = "The user has been removed successfully!";
            return RedirectToAction("Confirmation", "Home");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["LoggedOut"] = "You have been logged out successfully!";
            return RedirectToAction("Login", "Home");
        }
    }
}
