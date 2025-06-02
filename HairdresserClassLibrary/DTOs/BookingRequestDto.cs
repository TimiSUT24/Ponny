namespace HairdresserClassLibrary.DTOs;

public class BookingRequestDto
{
    public string HairdresserId { get; set; } = null!;
    public int TreatmentId { get; set; }
    public DateTime Start { get; set; }

}
