using ActionApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ActionApp.Data
{
	public class AuctionAppDbContext : DbContext
	{
        public AuctionAppDbContext(DbContextOptions<AuctionAppDbContext> options) : base(options)
        {

        }

        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<User> Users { get; set; }

		//mapping the auctiong to the startingprice and bid to the amount defining the decimal
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure precision and scale for decimal properties
			modelBuilder.Entity<Auction>(entity =>
			{
				entity.Property(e => e.StartingPrice)
					.HasColumnType("decimal(18, 2)"); // Adjust precision and scale as needed
			});

			modelBuilder.Entity<Bid>(entity =>
			{
				entity.Property(e => e.Amount)
					.HasColumnType("decimal(18, 2)"); // Adjust precision and scale as needed
			});

			base.OnModelCreating(modelBuilder);
		}
	}
}
