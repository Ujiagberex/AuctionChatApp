using ActionApp.Data;
using ActionApp.Interfaces;
using ActionApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ActionApp.Repository
{
    public class AuctionRepo : IAuctionRepo
    {
        private readonly AuctionAppDbContext _context;

        public AuctionRepo(AuctionAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Auction>> GetAllAsync()
        {
            return await _context.Auctions.Include(a => a.Bids).ToListAsync();
        }

        public async Task<Auction> GetByIdAsync(int id)
        {
            return await _context.Auctions.Include(a => a.Bids).FirstOrDefaultAsync(a => a.AuctionId == id);
        }

        public async Task AddAsync(Auction auction)
        {
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Auction auction)
        {
            _context.Entry(auction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction != null)
            {
                _context.Auctions.Remove(auction);
                await _context.SaveChangesAsync();
            }

        }
    }
}
