using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CollegeApp_2.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Student");

            builder.HasKey(x => x.Id);  // Student sinifinda KEY ibaresini kaldirabiliriz burada tanimliyoruz.

            builder.Property(n => n.Id).UseIdentityColumn(); // Student sinifinda DatabaseGenerated(DatabaseGeneratedOption.Identity) ibaresini kaldirabiliriz burada tanimliyoruz.

            builder.Property(n => n.StudentName).IsRequired();   // StudentName gereklidir.
            builder.Property(n => n.Email).HasMaxLength(250);     // Maxsimim 250 karakter.
            builder.Property(n => n.Adres).IsRequired(false).HasMaxLength(500);    // Adres istege bagli.Maksimum 500 karakter.
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);         // Email gereklidir.Maksimum 250 karakter.


            builder.HasData(new List<Student>
            {
                new Student
                {
                    Id = 1,
                    StudentName = "Yunus",
                    Email ="yunus@hotmail.com",
                    Adres = "Hyd, INDIA",
                    DOB = new DateTime(2022,12,12)


                },
                new Student
                {
                    Id = 2,
                    StudentName = "Anil",
                    Email = "Anil@hotmail.com",
                    Adres = "Banglore, INDIA",
                    DOB = new DateTime(2022,06,12)

                } 
            });
        }
    }
}
