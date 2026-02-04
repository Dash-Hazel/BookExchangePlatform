using BookExchangePlatform.Models;
using Microsoft.EntityFrameworkCore;
namespace BookExchangePlatform.Data
{
    public class BookExchangeDbContext: DbContext
    {
        //This file may  need configuration in the future.
        // TODO: Check if the Delete Behavior should be Restricted.
        


        public BookExchangeDbContext(DbContextOptions<BookExchangeDbContext> options)
    : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExchangeBook> ExchangeBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<ExchangeBook>()
                .HasOne(e => e.Owner)
                .WithMany()  
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<ExchangeBook>()
                .HasOne(e => e.Requester)
                .WithMany(u => u.RequestedExchanges) 
                .HasForeignKey(e => e.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<ExchangeBook>()
                .HasOne(e => e.OfferedBook)
                .WithMany() 
                .HasForeignKey(e => e.OfferedBookId)
                .OnDelete(DeleteBehavior.Restrict);

           
            modelBuilder.Entity<ExchangeBook>()
                .HasOne(e => e.RequestedBook)
                .WithMany() 
                .HasForeignKey(e => e.RequestedBookId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Book>()
             .HasOne(b => b.Owner)
              .WithMany(u => u.OwnedBooks) 
             .HasForeignKey(b => b.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
