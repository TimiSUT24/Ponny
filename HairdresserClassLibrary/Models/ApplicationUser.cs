using Microsoft.AspNetCore.Identity;

namespace HairdresserClassLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // ...existing code...
        public ICollection<Booking> HairdresserBookings { get; set; } = [];
        public ICollection<Booking> CustomerBookings { get; set; } = [];
        // ...existing code...
    }
}
