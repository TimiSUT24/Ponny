using Hairdresser.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping;

public static class TreatmentMapper
{
    public static TreatmentDTO MapToTreatmentDto(this Treatment treatment)
    {
        ArgumentNullException.ThrowIfNull(treatment);

        return new TreatmentDTO
        {
            Id = treatment.Id,
            Name = treatment.Name,
            Duration = treatment.Duration,
            Price = treatment.Price
        };
    }
}
