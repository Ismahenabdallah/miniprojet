using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace atelier3.Models
{
    public class AppDbContext: IdentityDbContext
    {

            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }
        
            public DbSet<Product> Products { get; set; }
        }

    }


