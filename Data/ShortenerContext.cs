using Shortener.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Shortener.Data
{
    public class ShortenerContext:IdentityDbContext<User, IdentityRole, string>
    {
        public ShortenerContext(DbContextOptions<ShortenerContext> options):base(options)
        {

        }
        public DbSet<ShortenerURL> ShortenerURLs { get; set; }
    }
}
