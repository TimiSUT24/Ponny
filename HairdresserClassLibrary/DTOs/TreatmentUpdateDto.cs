namespace HairdresserClassLibrary.DTOs;

public class TreatmentUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double Price { get; set; }
}
