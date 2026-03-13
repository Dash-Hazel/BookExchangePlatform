using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using BookExchangePlatform.Common;

namespace BookExchangePlatform.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.ReviewTitleMinLength)]
        [MaxLength(ValidationConstants.ReviewTitleMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        [Range(ValidationConstants.ReviewMinRating, ValidationConstants.ReviewMaxRating)]
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string OwnerId { get; set; } = null!;
        
        public int? MovieId { get; set; } = null!;
        
        public int? BookId { get; set; } = null!;


        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        [ForeignKey("MovieId")]
        public Movie? Movie { get; set; } = null!;

        [ForeignKey("BookId")]
        public Book? Book { get; set; } = null!;
    }
}
