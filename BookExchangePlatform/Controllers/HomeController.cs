using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BookExchangePlatform.Controllers
{
    public class HomeController : Controller
    {

        private readonly BookExchangeDbContext currContext;

        public HomeController(BookExchangeDbContext context)
        {
            currContext = context;
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
