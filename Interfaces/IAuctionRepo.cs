using ActionApp.Models;

namespace ActionApp.Interfaces
{
	public interface IAuctionRepo
	{
		//
		Task<IEnumerable<Auction>> GetAllAsync();
		Task<Auction> GetByIdAsync(int id);
		Task AddAsync(Auction auction);
		Task UpdateAsync(Auction auction);
		Task DeleteAsync(int id);

	}
}
