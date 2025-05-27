using Hairdresser.Data;
using Hairdresser.Repositories;
using Hairdresser.Repositories.Interfaces;
using Hairdresser.Services;
using Hairdresser.Services.Interfaces;
using HairdresserClassLibrary.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi(options => options.AddDocumentTransformer(new BearerSecuritySchemeTransformer()));


builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSeeding((context, _) =>
        {
            // BookingSeed.SeedBookings(context);
            TreatmentSeed.SeedTreatments(context);
        })
        .UseAsyncSeeding(async (context, _, _) =>
        {
            // await BookingSeed.SeedBookingsAsync(context);
            await TreatmentSeed.SeedTreatmentsAsync(context);
        }));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders()
                .AddApiEndpoints();

builder.Services.AddScoped<JWT_Service>();
builder.Services.AddScoped<IHairdresserService, HairdresserService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IHairdresserRepository, HairdresserRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();

//JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.Authority = jwtSettings["Authority"];
        options.Audience = jwtSettings["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });
builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IGenericRepository<Treatment>, TreatmentRepository>();
builder.Services.AddScoped<IGenericRepository<Booking>, BookingRepository>();
builder.Services.AddScoped<IGenericRepository<ApplicationUser>, UserRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

// Seed users and roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDBContext>();

    await context.Database.MigrateAsync();
    await UserRolesSeed.SeedUsersRoles(services);
    await BookingSeed.SeedBookingsAsync(services);
}

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// app.MapIdentityApi<ApplicationUser>();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
