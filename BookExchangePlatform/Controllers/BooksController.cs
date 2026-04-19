using BookExchangePlatform.Common;
using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Providers;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace BookExchangePlatform.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService currBookService;
        private readonly BookExchangeDbContext currContext;
        private readonly UserManager<User> userrManager;

        public BooksController(IBookService bookService, BookExchangeDbContext context, UserManager<User> userManager)
        {
            currBookService = bookService;
            currContext = context;
            userrManager = userManager;
        }

        private void PopulateUsersDropdown()
        {
            var users = currContext.Users.ToList();
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


        public async Task<IActionResult> Index(string? search, int page = 1)
        {
            var query = currContext.Books.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }
            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(await currContext.Books.CountAsync() / 10.0);

            var books = await currBookService.GetAllBooksAsync(search, page);
            return View(books);
        }

        public IActionResult Create()
        {
            PopulateUsersDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            ModelState.Remove("Owner");

            if (book.DateOfPublishing == default)
            {
                book.DateOfPublishing = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                await currBookService.CreateBookAsync(book);
                return RedirectToAction(nameof(Index));
            }

            PopulateUsersDropdown();
            return View(book);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await currBookService.GetBookByIdAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            var userId = userrManager.GetUserId(User);
            if (book.OwnerId != userId)
                return Forbid();

            PopulateUsersDropdown();
            return View(book);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {

            if (id != book.Id)
            {
                return NotFound();
            }

            ModelState.Remove("OwnerId");
            ModelState.Remove("Owner");
           

            if (ModelState.IsValid)
            {
              var updatedBook = await currBookService.UpdateBookAsync(id, book);

                if (updatedBook == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }

            PopulateUsersDropdown();
            return View(book);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await currBookService.GetBookWithOwnerAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }

            var userId = userrManager.GetUserId(User);
            if (book.OwnerId != userId)
                return Forbid();

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await currBookService.GetBookByIdAsync(id);
            if (book == null) return NotFound();

            var userId = userrManager.GetUserId(User);
            if (book.OwnerId != userId)
            {
                return Forbid();
            }
                
            await currBookService.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.BookId = id;
            if (id == null)
            {
                return NotFound();
            }

            var book = await currBookService.GetBookWithOwnerAsync(id.Value);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

    }
}
