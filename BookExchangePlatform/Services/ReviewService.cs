using BookExchangePlatform.Data;
using BookExchangePlatform.Services.Interfaces;
using BookExchangePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Services
{
    public class ReviewService : IReviewService
    {
        private readonly BookExchangeDbContext currContext;

        public ReviewService(BookExchangeDbContext context)
        {
            currContext = context;
        }

        public async Task<List<Review>> GetBookReviewsAsync(int bookId)
        {
            return await currContext.Reviews
                .Where(r => r.BookId == bookId)
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<List<Review>> GetMovieReviewsAsync(int movieId)
        {
            return await currContext.Reviews
                .Where(r => r.MovieId == movieId)
                .Include(r => r.Owner)
                .ToListAsync();
        }

        public async Task<Review?> GetReviewByIdAsync(int id)
        {
            return await currContext.Reviews
                .Where(r => r.Id == id)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync();
        }

        public async Task<Review> CreateReviewAsync(Review review)
        {
            if (review.OwnerId == null)
            {
                throw new ArgumentException("Review must have an owner.");
            }

            currContext.Reviews.Add(review);
            var saveResult = await currContext.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var review = await currContext
                .Reviews
                .Where(r => r.Id == id)
                .FirstOrDefaultAsync();

            if (review == null)
            {
                return false;
            }

            currContext.Reviews.Remove(review);
            var saveResult = await currContext.SaveChangesAsync();
            return true;
        }
    }
}
