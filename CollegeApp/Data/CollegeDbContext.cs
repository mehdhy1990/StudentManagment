using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data
{
    public class CollegeDbContext : DbContext
    {
        private DbSet<Student> Students { get; set; }
        public CollegeDbContext(DbContextOptions<CollegeDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>().HasData(new List<Student>()
            {
                new Student(){Id = 1,StudentName = "mehdi",Address = "Abhar",Email = "mehdi@gmail.com",DOB = new DateTime(2022,12,2)},
                new Student(){Id = 2,StudentName = "merss",Address = "Rasht",Email = "merss@gmail.com",DOB = new DateTime(2022,10,5)}
            });
        }
    }
}
