using System;

namespace Hairdresser.DTOs;

public class TreatmentDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double Price { get; set; }
}
