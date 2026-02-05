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
        [MinLength(ValidationConstants.TitleMinLength)]
        [MaxLength(ValidationConstants.TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.AuthorMinLength)]
        [MaxLength(ValidationConstants.AuthorMaxLength)]
        public string Author { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.DescriptionMinLength)]
        [MaxLength(ValidationConstants.DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string Genre { get; set; } = null!;

        [Required]
        public string Condition { get; set; } = "Good";

        public bool IsAvailable { get; set; } = true;

        [Required]
        public DateTime DateOfPublishing { get; set; } = DateTime.Now;

        //Navigation properties

        [Required(ErrorMessage = "Please select an owner")]
        [Display(Name = "Owner")]
        public int OwnerId { get; set; }

        
        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;

        
    }
}
