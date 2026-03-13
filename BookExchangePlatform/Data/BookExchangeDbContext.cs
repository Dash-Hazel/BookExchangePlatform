using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookExchangePlatform.Models;

namespace BookExchangePlatform.Data
{
    public class BookExchangeDbContext : IdentityDbContext<User>
    {
        //This file may  need configuration in the future.
        // TODO: Check if the Delete Behavior should be Restricted.



        public BookExchangeDbContext(DbContextOptions<BookExchangeDbContext> options)
    : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> WishLists{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Movie>()
                .HasOne(e => e.Owner)
                .WithMany(u => u.Movies)
                .HasForeignKey(e => e.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
             .HasOne(b => b.Owner)
              .WithMany(u => u.Books)
             .HasForeignKey(b => b.OwnerId)
             .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Owner)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.WishLists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
