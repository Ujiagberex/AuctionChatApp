using System.ComponentModel.DataAnnotations;

namespace ActionApp.Models
{
	public class User
	{
		[Key]
        public int UserId { get; set; }

		[Required]	
        public string Username { get; set; }
    }
}
