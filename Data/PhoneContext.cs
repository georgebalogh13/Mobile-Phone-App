using Microsoft.EntityFrameworkCore;
using Phone_project_API.Models;

namespace Phone_project_API.Data
{
    public class PhoneContext : DbContext
    {
        public PhoneContext(DbContextOptions<PhoneContext> options) : base(options)
        {
        }

        public DbSet<Phone> Phones { get; set; }
    }
}
