using ActionApp.Interfaces;
using ActionApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ActionApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuctionAppController : ControllerBase
	{
		private readonly IAuctionRepo _auctionRepo;

		public AuctionAppController(IAuctionRepo auctionRepo)
		{
			_auctionRepo = auctionRepo;
		}

		[HttpGet]
		public async Task<IEnumerable<Auction>> GetAll()
		{
			return await _auctionRepo.GetAllAsync();
		}

		[HttpGet("{id}")]
		public async Task<Auction> GetById(int id)
		{
			return await _auctionRepo.GetByIdAsync(id);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Auction auction)
		{
			await _auctionRepo.AddAsync(auction);
			return CreatedAtAction(nameof(GetById), new { id = auction.AuctionId }, auction);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Auction auction)
		{
			if (id != auction.AuctionId)
			{
				return BadRequest();
			}

			await _auctionRepo.UpdateAsync(auction);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _auctionRepo.DeleteAsync(id);
			return NoContent();
		}
	}
}
