using Hairdresser.Data;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, _) =>
        {
            UserSeed.SeedUsers(context);
            BookingSeed.SeedBookings(context);
            TreatmentSeed.SeedTreatments(context);
        })
        .UseAsyncSeeding(async (context, _, _) =>
        {
            await UserSeed.SeedUsersAsync(context);
            await BookingSeed.SeedBookingsAsync(context);
            await TreatmentSeed.SeedTreatmentsAsync(context);
        }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();


//Repositories
builder.Services.AddScoped<IGenericRepository<Treatment>, TreatmentRepository>();
builder.Services.AddScoped<IGenericRepository<Booking>, BookingRepository>();

//Services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.MapScalarApiReference();
}

app.MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();