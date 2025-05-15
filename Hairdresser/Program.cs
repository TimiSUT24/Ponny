using Hairdresser.Data;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>()
    .AddDefaultTokenProviders();

// Repositories
builder.Services.AddScoped<IGenericRepository<Treatment>, TreatmentRepository>();
builder.Services.AddScoped<IGenericRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IGenericRepository<ApplicationUser>, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
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
