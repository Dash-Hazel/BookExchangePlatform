namespace BookExchangePlatform.Models
{
    public class HomeView
    {
        public int BookCount { get; set; }
        public int MovieCount { get; set; }

        public List<Book> Books { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
