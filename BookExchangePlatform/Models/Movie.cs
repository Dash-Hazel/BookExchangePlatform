using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using BookExchangePlatform.Common;
using BookExchangePlatform.Common;
namespace BookExchangePlatform.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.MovieTitleMinLength)]
        [MaxLength(ValidationConstants.MovieTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.DirectorMinLength)]
        [MaxLength(ValidationConstants.DirectorMaxLength)]
        public string Director { get; set; } = null!;

        [Required]
        public DateOnly ReleaseYear { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Required]
        [MinLength(ValidationConstants.GenreMinLength)]
        [MaxLength(ValidationConstants.GenreMaxLength)]
        public string Genre { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.MovieResumeMinLength)]
        [MaxLength(ValidationConstants.MovieResumeMaxLength)]
        public string Resume { get; set; } = null!;

        [Required]
        public int OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;
    }
}
