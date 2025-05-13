using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hairdresser.Models;

public class HairdresserContext : IdentityDbContext<IdentityUser>
{
    public HairdresserContext(DbContextOptions<HairdresserContext> options)
        : base(options)
    {
    }
}