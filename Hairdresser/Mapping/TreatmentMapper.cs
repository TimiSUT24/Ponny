using HairdresserClassLibrary.DTOs;
using HairdresserClassLibrary.Models;

namespace Hairdresser.Mapping;

public static class TreatmentMapper
{
    public static TreatmentDto MapToTreatmentDto(this Treatment treatment)
    {
        ArgumentNullException.ThrowIfNull(treatment);

        return new TreatmentDto
        {
            Id = treatment.Id,
            Name = treatment.Name,
            Duration = treatment.Duration,
            Price = treatment.Price
        };
    }
}
