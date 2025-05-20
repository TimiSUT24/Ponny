namespace Hairdresser.DTOs
{
    public class BookingRequestDto
    {
        public string CustomerId { get; set; } = null!;
        public string HairdresserId { get; set; } = null!;
        public int TreatmentId { get; set; }
        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
