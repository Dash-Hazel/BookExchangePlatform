using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookExchangePlatform.Controllers
{
    public class HomeController : Controller
    {

        private readonly BookExchangeDbContext currContext;
        private readonly SignInManager<User> ssignInManager;

        public HomeController(BookExchangeDbContext context, SignInManager<User> signInManager)
        {
            currContext = context;
            ssignInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404) return View("Error404");
            else if (statusCode == 500) return View("Error500");
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var viewModel = new HomeView
                {
                    BookCount = await currContext.Books.CountAsync(),
                    MovieCount = await currContext.Movies.CountAsync(),

                    Books = await currContext.Books
                    .Include(b => b.Owner)
                    .OrderByDescending(b => b.Title)
                    .ToListAsync(),

                    Movies = await currContext.Movies
                    .Include(b => b.Owner)
                    .OrderByDescending(b => b.Title)
                    .ToListAsync()
                };
                return View(viewModel);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await ssignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
