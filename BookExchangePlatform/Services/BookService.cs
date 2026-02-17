using BookExchangePlatform.Data;
using BookExchangePlatform.Models;
using BookExchangePlatform.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.CodeAnalysis.CSharp.SyntaxTokenParser;

namespace BookExchangePlatform.Services
{
    public class BookService : IBookService
    {





        private readonly BookExchangeDbContext currContext;

        public BookService(BookExchangeDbContext context)
        {
            currContext = context;
        }
        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await currContext.Books
                  .Include(b => b.Owner)
                  .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await currContext.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Book?> GetBookWithOwnerAsync(int id)
        {
            return await currContext.Books
                .Include(b => b.Owner)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            if (book.OwnerId == null)
            {
                var user = await currContext.Users.FirstAsync();
                book.OwnerId = user.Id;
            }

            currContext.Books.Add(book);

            var saveResult = await currContext.SaveChangesAsync();
            return book;

        }
        public async Task<Book?> UpdateBookAsync(int id, Book book)
        {
            if (book.OwnerId == null)
            {
                var originalBook = await currContext.Books
                    .Where(o => o.Id == id)
                    .FirstOrDefaultAsync();

                if (originalBook != null)
                {
                    book.OwnerId = originalBook.OwnerId;
                }
            }

            var currBook = await currContext.Books.FindAsync(id);

            if (currBook == null)
            {
                return null;
            }


            currBook.Title = book.Title;
            currBook.Author = book.Author;
            currBook.Description = book.Description;
            currBook.Genre = book.Genre;
            currBook.Condition = book.Condition;
            currBook.DateOfPublishing = book.DateOfPublishing;
            currBook.OwnerId = book.OwnerId;
            await currContext.SaveChangesAsync();
            return currBook;
        }

        public async Task<bool> DeleteBookAsync(int id)
        {
            Console.WriteLine($"Attempting to delete book with ID: {id}");
            var book = await currContext.Books
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();

            if (book == null)
            {
                Console.WriteLine("Book not found");
                return false;
            }

            Console.WriteLine($"Found book: {book.Title}");
            currContext.Remove(book);
            var result = await currContext.SaveChangesAsync();
            Console.WriteLine($"SaveChanges result: {result}");
            return true;

        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await currContext.Users.ToListAsync();

        }

        public async Task<User?> GetFirstUserAsync()
        {
            return await currContext.Users.FirstOrDefaultAsync();
        }
    }
}
