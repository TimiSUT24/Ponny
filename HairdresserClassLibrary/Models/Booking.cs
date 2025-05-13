namespace HairdresserClassLibrary.Models;


public class Booking
{
	public int Id { get; set; }
	public DateTime Start { get; set; }
	public DateTime End { get; set; }
	public ApplicationUser Customer { get; set; } = null!;
	public ApplicationUser Hairdresser { get; set; } = null!;
	public Treatment Treatment { get; set; } = null!;

}
