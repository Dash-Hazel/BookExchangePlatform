using BookExchangePlatform.Common;
using BookExchangePlatform.Data;
using BookExchangePlatform.Migrations;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
namespace BookExchangePlatform.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookExchangeDbContext context;
        public BooksController(BookExchangeDbContext context)
        {
            this.context = context;
        }

        private void PopulateUsersDropdown()
        {
            var users = context.Users.ToList();
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

        public IActionResult Index()
        {
            var books = context.Books
                .Include(b => b.Owner)
                .ToList();
            return View(books);
        }

        public IActionResult Create()
        {
            PopulateUsersDropdown();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {            

            // Ensure OwnerId is valid
            if (book.OwnerId == 0)
            {
                var firstUser = context.Users.First();
                book.OwnerId = firstUser.Id;
            }

            context.Add(book);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }



        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            PopulateUsersDropdown();
            return View(book);

        }

        [HttpPost]
        public IActionResult Edit(int id, Book book)
        { 

            if (book.Id != id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                context.Update(book);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            PopulateUsersDropdown();
            return View(book);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = context.Books.Find(id);
            
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {

            var book = context.Books.Find(id);
            if (book != null)
            {
                context.Remove(book);
                context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
          

            if (id == null)
            {
                return NotFound();
            }

            var book = context.Books
                .Include(b => b.Owner)
                .FirstOrDefault(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

    }
}
