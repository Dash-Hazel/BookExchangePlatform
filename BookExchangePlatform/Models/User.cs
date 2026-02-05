using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using BookExchangePlatform.Common;
namespace BookExchangePlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.FirstNameMinLength)]
        [MaxLength(ValidationConstants.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.LastNameMinLength)]
        [MaxLength(ValidationConstants.LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.EmailMinLength)]
        [MaxLength(ValidationConstants.EmailMaxLength)]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Location { get; set; } = null!;


        public string FullName => $"{FirstName} {LastName}";

        //Navigation properties       
        public ICollection<ExchangeBook> RequestedExchanges { get; set; } = new List<ExchangeBook>();

        public ICollection<Book> OwnedBooks { get; set; } = new List<Book>();
    }
}
