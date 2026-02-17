using BookExchangePlatform.Models;

namespace BookExchangePlatform.Services.Interfaces

{
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(int id);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book> CreateBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id, Book book);
        Task<bool> DeleteBookAsync(int id);
        
        Task<Book> GetBookWithOwnerAsync(int id);


        //UserListPopulation
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetFirstUserAsync();

    }
}
