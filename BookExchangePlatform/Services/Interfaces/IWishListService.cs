using BookExchangePlatform.Models;
namespace BookExchangePlatform.Services.Interfaces
{
    public interface IWishListService
    {
        Task<List<Wishlist>> GetUserWishListAsync(string userId);
        Task<Wishlist> AddItemToWishListAsync(string userId, int? bookId, int? movieId);
        Task<bool> RemoveFromWishListAsync(int id);
        Task<bool> ExistsInWishList(string userId, int? bookId, int? movieId);
    }
}
