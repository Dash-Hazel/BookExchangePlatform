using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Areas.Admin.Controllers
{
    
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> userrManager;

        public AdminController(UserManager<User> userManager)
        {
            userrManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userrManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Users()
        {
            var users = await userrManager.Users.ToListAsync();
            return View(users);
        }
    }
}
