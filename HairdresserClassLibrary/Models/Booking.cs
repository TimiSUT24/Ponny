namespace HairdresserClassLibrary.Models;


public class Booking
{
	public int Id { get; set; }
	public DateTime Start { get; set; }
	public DateTime End { get; set; }

	// Navigation properties
	// // public ApplicationUser Customer { get; set; } = null!;
	// // public ApplicationUser Hairdresser { get; set; } = null!;
	// // public Treatment Treatment { get; set; } = null!;

	public int CustomerId { get; set; }
	public int HairdresserId { get; set; }
	public int TreatmentId { get; set; }
	public ApplicationUser Customer { get; set; } = null!;
	public ApplicationUser Hairdresser { get; set; } = null!;
	public Treatment Treatment { get; set; } = null!;
}
