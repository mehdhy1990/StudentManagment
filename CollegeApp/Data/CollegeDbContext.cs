using CollegeApp.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new StudentConfig());
        }
    }
}
