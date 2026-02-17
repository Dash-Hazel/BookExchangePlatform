using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService currMovieService;
        private readonly BookExchangeDbContext currContext;

        public MoviesController(IMovieService movieService, BookExchangeDbContext context)
        {
            currMovieService = movieService;
            currContext = context;
        }

        private void PopulateUsersDropdown()
        {
            var users = currContext.Users.ToList();
            var userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                userList.Add(new SelectListItem
                {
                    Value = user.Id.ToString(),
                    Text = $"{user.FirstName} {user.LastName}"
                });
            }

            ViewBag.Users = userList;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await currMovieService.GetAllMoviesAsync();
            return View(movies);
        }

        public IActionResult Create()
        {
            PopulateUsersDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Movie movie)
        {
            ModelState.Remove("Owner");

            if (movie.ReleaseYear == default)
            {
                movie.ReleaseYear = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                await currMovieService.CreateMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }

            PopulateUsersDropdown();
            return View(movie);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await currMovieService.GetMovieByIdAsync(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            PopulateUsersDropdown();
            return View(movie);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {

            if (id != movie.Id)
            {
                return NotFound();
            }

            ModelState.Remove("OwnerId");
            ModelState.Remove("Owner");


            if (ModelState.IsValid)
            {
                var updatedMovie = await currMovieService.UpdateMovieAsync(id, movie);

                if (updatedMovie == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateUsersDropdown();
            return View(movie);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie= await currMovieService.GetMovieWithOwnerAsync(id.Value);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            await currMovieService.DeleteMovieAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var book = await currMovieService.GetMovieWithOwnerAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }
    }
}
