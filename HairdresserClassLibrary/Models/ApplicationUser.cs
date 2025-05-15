using Microsoft.AspNetCore.Identity;

namespace HairdresserClassLibrary.Models
{
	public class ApplicationUser : IdentityUser
	{
		public ICollection<Booking> Bookings { get; set; } = [];
	}
}