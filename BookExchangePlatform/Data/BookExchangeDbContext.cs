using BookExchangePlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace BookExchangePlatform.Data
{
    public class BookExchangeDbContext: DbContext
    {
        public BookExchangeDbContext(DbContextOptions<BookExchangeDbContext> options)
    : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExchangeBook> ExchangeBooks { get; set; }
    }
}
