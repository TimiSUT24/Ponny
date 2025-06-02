namespace HairdresserClassLibrary.DTOs;

public class TreatmentCreateUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double Price { get; set; }
}
