using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookExchangePlatform.Controllers
{
    [Authorize]
    public class WishListsController : Controller
    {

        private readonly IWishListService currWishListService;
        private readonly BookExchangeDbContext currContext;
        private readonly UserManager<User> userManager;


        public WishListsController(IWishListService wishListService, BookExchangeDbContext context, UserManager<User> userManager)
        {
            currWishListService = wishListService;
            currContext = context;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserId(User);
            var wishLists = await currWishListService.GetUserWishListAsync(userId);
            return View(wishLists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToWishList(int? bookId, int? movieId)
        {
            if (ModelState.IsValid)
            {
                var userId = userManager.GetUserId(User);
                await currWishListService.AddItemToWishListAsync(userId, bookId, movieId);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItemFromWishList(int id)
        {
            await currWishListService.RemoveFromWishListAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
