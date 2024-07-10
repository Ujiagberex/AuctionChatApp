using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActionApp.Models
{
	public class Bid
	{
		[Key]
        public int BidId { get; set; }
		public decimal Amount { get; set; }
		
		[ForeignKey("AuctionId")]
		public int AuctionId { get; set; }
		public Auction Auction { get; set; }
		
		[ForeignKey("UserId")]
		public int UserId { get; set; }
		public User User { get; set; }
	}
}
