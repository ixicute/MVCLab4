using Lab4.CustomIdentity;
using Lab4.Data;
using Lab4.Models;
using Lab4.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CustomUserManager userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext _context, SignInManager<User> _signInManager, CustomUserManager _userManager)
        {
            context = _context;
            signInManager = _signInManager;
            userManager = _userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var response = new LoginViewModel();

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var user = await userManager.FindByEmailAsync(loginViewModel.Email);

            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ViewData["Error"] = "Wrong credentials. Please, try again";
                return View(loginViewModel);
            }
            ViewData["Error"] = "Wrong credentials. Please, try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var user = await userManager.FindByEmailAsync(registerViewModel.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "This email already exists. Try again.");
                return View(registerViewModel);
            }

            var username = await context.Users.FirstOrDefaultAsync(u => u.UserName == registerViewModel.Username);
            if (username != null)
            {
                ModelState.AddModelError("Username", "This username is already taken. Try again.");
                return View(registerViewModel);
            }

            var address = await context.Addresses.FirstOrDefaultAsync(a => a.Street.ToLower() == registerViewModel.Street.ToLower() && a.City.ToLower() == registerViewModel.City.ToLower());
            if (address == null)
            {
                address = new Address
                {
                    Street = registerViewModel.Street,
                    City = registerViewModel.City,
                    PostalCode = registerViewModel.PostalCode
                };
                await context.Addresses.AddAsync(address);
                await context.SaveChangesAsync();
            }


            var newUser = new User()
            {
                FirstName = registerViewModel.Firstname,
                LastName = registerViewModel.Lastname,
                FK_AddressId = address.Id,
                FK_RoleId = registerViewModel.roleType,
                UserName = registerViewModel.Username,
                Email = registerViewModel.Email,
                EmailConfirmed = true
            };

            var newUserResponse = await userManager.CreateAsync(newUser, registerViewModel.Password);


            if (newUserResponse.Succeeded)
            {
                TempData["Confirm"] = "The user has been created successfully!";
                return RedirectToAction("Confirmation", "Home");
            }
            TempData["Confirm"] = "There was an error. Please try again.";
            return RedirectToAction("Confirmation", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}