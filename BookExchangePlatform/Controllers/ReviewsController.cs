using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookExchangePlatform.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {

        private readonly IReviewService currReviewService;
        private readonly BookExchangeDbContext currContext;
        private readonly UserManager<User> userManager;


        public ReviewsController(IReviewService reviewtService, BookExchangeDbContext context, UserManager<User> userManager)
        {
            currReviewService = reviewtService;
            currContext = context;
            this.userManager = userManager;
        }

        public IActionResult Create(int? bookId, int? movieId)
        {
            ViewBag.bookId = bookId;
            ViewBag.movieId = movieId;  
            return View();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            if (id == null) return NotFound();

            var review = await currReviewService.GetReviewByIdAsync(id.Value);

            if (review == null) return NotFound();

            var userId = userManager.GetUserId(User);
            if (review.OwnerId != userId)
                return Forbid();

            return View(review);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Review review)
        {
            ModelState.Remove("Owner");
            ModelState.Remove("Book");
            ModelState.Remove("Movie");
            ModelState.Remove("OwnerId");

            review.OwnerId = userManager.GetUserId(User);
            review.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                await currReviewService.CreateReviewAsync(review);

                if (review.BookId == null)
                    return RedirectToAction("Details", "Movies", new { id = review.MovieId });
                else
                    return RedirectToAction("Details", "Books", new { id = review.BookId });
            }

            return View(review);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Console.WriteLine($"DeleteConfirmed HIT with id: {id}");
            var review = await currReviewService.GetReviewByIdAsync(id);

            if (review == null) return NotFound();

            await currReviewService.DeleteReviewAsync(id);

            if (review.BookId == null)
             return RedirectToAction("Details", "Movies", new { id = review.MovieId });

            else
             return RedirectToAction("Details", "Books", new { id = review.BookId });


        }
    }
}
