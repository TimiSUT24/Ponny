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
            Description = treatment.Description ?? string.Empty,
            Duration = treatment.Duration,
            Price = treatment.Price
        };
    }
    public static CreateTreatmentDTO MapToCreateTreatmentDto(this Treatment treatment)
    {
        ArgumentNullException.ThrowIfNull(treatment);

        return new CreateTreatmentDTO
        {
            Name = treatment.Name,
            Description = treatment.Description,
            Duration = treatment.Duration,
            Price = treatment.Price
        };
    }

    public static Treatment MapToTreatment(this CreateTreatmentDTO createTreatmentDto)
    {
        ArgumentNullException.ThrowIfNull(createTreatmentDto);

        return new Treatment
        {
            Name = createTreatmentDto.Name,
            Description = createTreatmentDto.Description,
            Duration = createTreatmentDto.Duration,
            Price = createTreatmentDto.Price
        };
    }

    public static Treatment MapToTreatment(this TreatmentUpdateDto treatment)
    {
        ArgumentNullException.ThrowIfNull(treatment);

        return new Treatment
        {
            Name = treatment.Name,
            Description = treatment.Description,
            Duration = treatment.Duration,
            Price = treatment.Price
        };
    }
}
