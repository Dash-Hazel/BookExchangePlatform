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
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Book book)
        {

            if (book.OwnerId == 0)
            {
                var originalBook = context.Books.Find(id);
                if (originalBook != null)
                {
                    book.OwnerId = originalBook.OwnerId;
                }
            }

            ModelState.Remove("OwnerId");
            ModelState.Remove("Owner");

            Console.WriteLine($"After removal - ModelState.IsValid: {ModelState.IsValid}");




            if (book.Id != id)
            {
                return NotFound();
            }
           

            if (ModelState.IsValid)
            {
                Book existingBook = context.Books
                    .Where(b => b.Id == id)
                    .FirstOrDefault();

                if (existingBook == null)
                {
                    return NotFound();
                }
                
                

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Description = book.Description;
                existingBook.Genre = book.Genre;
                existingBook.Condition = book.Condition;
                existingBook.IsAvailable = book.IsAvailable;
                existingBook.DateOfPublishing = book.DateOfPublishing;
                existingBook.OwnerId = book.OwnerId;
                context.SaveChanges();
                return RedirectToAction(nameof(Index));

            }

            Console.WriteLine("Remaining errors after removal:");
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"{state.Key}: {error.ErrorMessage}");
                }
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
