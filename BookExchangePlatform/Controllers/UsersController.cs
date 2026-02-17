using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;

namespace BookExchangePlatform.Controllers
{

    public class UsersController : Controller
    {

        private readonly IUserService currUserService;
        private readonly BookExchangeDbContext currContext;
        private readonly UserManager<User> currUserManager;

        public UsersController(IUserService userService, BookExchangeDbContext context, UserManager<User> userManager)
        {
            currUserService = userService;
            currContext = context;
            currUserManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
           var users = await currUserService.GetAllUsersAsync();
            return View(users);
        }

        [Authorize]
        public async Task<IActionResult> MyPublications()
        {


            var currUserId = currUserManager.GetUserId(User);

            var viewModel = new MyPublications
            {
                Books = await currContext.Books
                    .Where(b => b.OwnerId == currUserId)
                    .Include(b => b.Owner)
                    .ToListAsync(),

                Movies = await currContext.Movies
                    .Where(b => b.OwnerId == currUserId)
                    .Include(b => b.Owner)
                    .ToListAsync(),


            };
            return View(viewModel);
            
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await currUserService.CreateUserAsync(user);
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await currUserService.GetUserWithDetailsAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await currUserService.GetUserWithDetailsAsync(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var deleedUser = await currUserService.DeleteUserAsync(id);
            if (deleedUser == false) return NotFound();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null) return NotFound();
            var user = await currUserService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                var updated = await currUserService.UpdateUserAsync(id, updatedUser);
                if (updated == null) return NotFound();
                return RedirectToAction(nameof(Index));
            }

            return View(updatedUser);
        }
    }
}
