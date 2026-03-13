using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Services
{
    public class WishListService: IWishListService
    {
        private readonly BookExchangeDbContext currContext;

        public WishListService(BookExchangeDbContext context)
        {
            currContext = context;
        }

        public async Task<List<Wishlist>> GetUserWishListAsync(string userId)
        {
            var user = await currContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found");

            return await currContext.WishLists
                .Where(w => w.UserId == userId)
                .ToListAsync();
        }

        public async Task<Wishlist> AddItemToWishListAsync(string userId, int? bookId, int? movieId)
        {
            var user =  await currContext
                .Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            if(user == null)
            {
                throw new Exception("User not found");
            }

            var wishList = new Wishlist
            {
                UserId = userId,
                BookId = bookId,
                MovieId = movieId,
                CreatedAt = DateTime.Now,
            };

            currContext.WishLists.Add(wishList);

            var saveResult = await currContext.SaveChangesAsync();

            return wishList;
        }

        public async Task<bool> RemoveFromWishListAsync(int id)
        {
            var wishlist = await currContext
                .WishLists
                .Where(w => w.Id == id)
                .FirstOrDefaultAsync();

            if (wishlist == null)
            {
                return false;
            }

           currContext.WishLists.Remove(wishlist);
           var saveResult = await currContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsInWishList(string userId, int? bookId, int? movieId)
        {
            var exists = await currContext.WishLists
        .Where(w => w.UserId == userId)
        .Where(w => w.BookId == bookId)
        .Where(w => w.MovieId == movieId)
        .AnyAsync();

            return exists;
        }
    }
}
