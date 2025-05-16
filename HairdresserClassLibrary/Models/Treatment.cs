using System;
using System.ComponentModel.DataAnnotations;

namespace HairdresserClassLibrary.Models;

public class Treatment
{
    public int Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public double Price { get; set; }

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
