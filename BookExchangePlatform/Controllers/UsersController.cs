using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Controllers
{
    public class UsersController : Controller
    {

        private readonly BookExchangeDbContext context;
        public UsersController(BookExchangeDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var users = context.Users
                .Include(u => u.OwnedBooks)
                .Include(u => u.RequestedExchanges)
                .ToList();

            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                context.Users.Add(user);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            User user = context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User user = context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            context.Users
                .Remove(user);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var user = context.Users
                .Where(u => u.Id == id)
                .FirstOrDefault();
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingUser = context.Users
                    .Where(e => e.Id == id)
                    .FirstOrDefault();
                if (existingUser == null) return NotFound();

                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.Email = updatedUser.Email;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Location = updatedUser.Location;
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(updatedUser);
        }
    }
}
