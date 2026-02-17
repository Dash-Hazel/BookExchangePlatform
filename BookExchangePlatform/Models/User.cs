using BookExchangePlatform.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
namespace BookExchangePlatform.Models
{
    public class User: IdentityUser
    {

        [Required]
        [MinLength(ValidationConstants.FirstNameMinLength)]
        [MaxLength(ValidationConstants.FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MinLength(ValidationConstants.LastNameMinLength)]
        [MaxLength(ValidationConstants.LastNameMaxLength)]
        public string LastName { get; set; } = null!;



        public string FullName => $"{FirstName} {LastName}";

        //Navigation properties       
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
