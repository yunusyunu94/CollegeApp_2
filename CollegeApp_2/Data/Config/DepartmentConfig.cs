using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data.Config
{
    public class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments"); // Tabloyu olustueduk

            builder.HasKey(x => x.Id);  // Student sinifinda KEY ibaresini kaldirabiliriz burada tanimliyoruz.

            builder.Property(n => n.Id).UseIdentityColumn(); // Student sinifinda DatabaseGenerated(DatabaseGeneratedOption.Identity) ibaresini kaldirabiliriz burada tanimliyoruz.

            builder.Property(n => n.DepartmentName).IsRequired().HasMaxLength(200);         // StudentName gereklidir.Maxsimim 200 karakter.
            builder.Property(n => n.Description).HasMaxLength(500).IsRequired(false);       // Maxsimim 500 karakter.


            builder.HasData(new List<Department>
            {
                new Department
                {
                    Id = 1,
                    DepartmentName = "ECE",
                    Description ="ECE Departmen",


                },
                new Department
                {
                    Id = 2,
                    DepartmentName = "CSE",
                    Description ="CSE Departmen",

                }
            });
        }
    }

}
