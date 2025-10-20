using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.StudentName).IsRequired();
            builder.Property(n => n.StudentName).HasMaxLength(250);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);
            builder.HasData(new List<Student>()
            {
                new Student(){Id = 1,StudentName = "mehdi",Address = "Abhar",Email = "mehdi@gmail.com",DOB = new DateTime(2022,12,2)},
                new Student(){Id = 2,StudentName = "merss",Address = "Rasht",Email = "merss@gmail.com",DOB = new DateTime(2022,10,5)}
            });
            builder.HasOne(n => n.Department).WithMany(n => n.Students).
                HasForeignKey(n => n.DepartmentId).HasConstraintName("FK_Students_Department");

        }
    }
}
