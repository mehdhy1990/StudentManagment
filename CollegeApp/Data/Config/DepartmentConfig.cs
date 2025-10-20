using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class DepartmentConfig: IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.DepartmentName).IsRequired().HasMaxLength(250);
            builder.Property(n => n.Description).HasMaxLength(500);
           
            builder.HasData(new List<Department>()
            {
                new Department(){Id = 1,DepartmentName = "Engineering",Description = "department for educating software engineers"},
                new Department(){Id = 2,DepartmentName = "Human science",Description = "department for education and cultivating future managers"}
            });

        }
    }

}
