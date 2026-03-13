using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookExchangePlatform.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } = null!;
        public int? BookId { get; set; }
        public int? MovieId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
        [ForeignKey("BookId")]
        public Book? Book { get; set; }
        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; }
    }
}
