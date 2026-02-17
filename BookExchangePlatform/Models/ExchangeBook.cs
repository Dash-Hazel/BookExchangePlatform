using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookExchangePlatform.Models
{
    public class ExchangeBook
    {
        public enum ExchangeStatus
        {
            Pending,
            Accepted,
            Rejected,
            Completed
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public int OfferedBookId { get; set; } //Being offered for exchange

        [Required]
        public int RequestedBookId { get; set; } //Being requested for exchange

        [Required]
        public ExchangeStatus Status { get; set; } = ExchangeStatus.Pending; //Pending, Accepted, Rejected, Completed

        [Required]
        public int RequesterId { get; set; } //Requester of the exchange

        [Required]
        public int OwnerId { get; set; } //Owner of the book

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

       
        [ForeignKey("RequesterId")]
        public User Requester { get; set; } = null!;

        
        [ForeignKey("OwnerId")]
        public User Owner { get; set; } = null!;


        
        [ForeignKey("OfferedBookId")]
        public Book OfferedBook { get; set; } = null!;

        
        [ForeignKey("RequestedBookId")]
        public Book RequestedBook { get; set; } = null!;
    }
}
