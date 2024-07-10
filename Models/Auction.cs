using System.ComponentModel.DataAnnotations;

namespace ActionApp.Models
{
	public class Auction
	{
		[Key]
		public int AuctionId { get; set; }
		[Required]
		[StringLength(150)]
		public string ItemName { get; set; }
		[Required]
		public decimal StartingPrice { get; set; }
		[Required]
		public DateTime StartTime { get; set; }
		[Required]
		public DateTime EndTime { get; set; }
		public List<Bid> Bids { get; set; }
	}
}
