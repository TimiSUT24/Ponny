using System;
using System.ComponentModel.DataAnnotations;

namespace HairdresserClassLibrary.DTOs;

public class CreateTreatmentDTO
{

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Duration is required.")]
    public int Duration { get; set; }
    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
    public double Price { get; set; }
}
