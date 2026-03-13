using BookExchangePlatform.Models;

namespace BookExchangePlatform.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<Review>> GetBookReviewsAsync(int bookId);
        Task<List<Review>> GetMovieReviewsAsync(int movieId);

        Task<Review?> GetReviewByIdAsync(int id);

        Task<Review> CreateReviewAsync(Review review);
        Task<bool> DeleteReviewAsync(int id);

    }
}
