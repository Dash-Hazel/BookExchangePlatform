using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BookExchangePlatform.Common;
namespace BookExchangePlatform.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(ValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.AuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        [Required]
        public string Condition { get; set; } = "Good";
        [Required]
        public bool IsAvailable { get; set; } = true;

        [Required]
        public DateTime DateOfPublishing { get; set; } = DateTime.UtcNow;


        //Navigation properties
        [Required]
        public int OwnerId { get; set; }

        
        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        
    }
}
