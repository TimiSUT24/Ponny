using System;
using System.Threading.Tasks;
using HairdresserClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Data;

public static class TreatmentSeed
{
    public static void SeedTreatments(DbContext context)
    {
        // modelBuilder.Entity<Treatment>().HasData(
        //     new Treatment()
        //     {
        //         Id = 1,
        //         Name = "Klippning",
        //         Description = "En professionell klippning för att ge ditt hår en ny stil.",
        //         Price = 500.00,
        //         Duration = 60,
        //     },
        //     new Treatment()
        //     {
        //         Id = 2,
        //         Name = "Färgning",
        //         Description = "En färgning av ditt hår för att ge det en ny look.",
        //         Price = 800.00,
        //         Duration = 90,
        //     },
        //     new Treatment()
        //     {
        //         Id = 3,
        //         Name = "Permanent",
        //         Description = "En permanent behandling för att ge ditt hår mer volym och lockar.",
        //         Price = 1200.00,
        //         Duration = 120,
        //     }
        // );
        var treatment = context.Set<Treatment>();
        List<Treatment> treatmentSeeds = [
             new Treatment()
            {
                Id = 1,
                Name = "Klippning",
                Description = "En professionell klippning för att ge ditt hår en ny stil.",
                Price = 500.00,
                Duration = 60,
            },
            new Treatment()
            {
                Id = 2,
                Name = "Färgning",
                Description = "En färgning av ditt hår för att ge det en ny look.",
                Price = 800.00,
                Duration = 90,
            },
            new Treatment()
            {
                Id = 3,
                Name = "Permanent",
                Description = "En permanent behandling för att ge ditt hår mer volym och lockar.",
                Price = 1200.00,
                Duration = 120,
            }
        ];

        if (!treatment.Any())
        {
            treatment.AddRange(treatmentSeeds);
            context.SaveChanges();
        }
    }

    public static async Task SeedTreatmentsAsync(DbContext context)
    {
        var treatment = context.Set<Treatment>();
        List<Treatment> treatmentSeeds = [
             new Treatment()
            {
                Name = "Klippning",
                Description = "En professionell klippning för att ge ditt hår en ny stil.",
                Price = 500.00,
                Duration = 60,
            },
            new Treatment()
            {
                Name = "Färgning",
                Description = "En färgning av ditt hår för att ge det en ny look.",
                Price = 800.00,
                Duration = 90,
            },
            new Treatment()
            {
                Name = "Permanent",
                Description = "En permanent behandling för att ge ditt hår mer volym och lockar.",
                Price = 1200.00,
                Duration = 120,
            }
        ];

        if (!treatment.Any())
        {
            await treatment.AddRangeAsync(treatmentSeeds);
            await context.SaveChangesAsync();
        }
    }
}
