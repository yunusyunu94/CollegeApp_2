using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
                
        }
        DbSet<Student> Students { get; set; }


       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Vari tabanina elle veri ekleme;

            modelBuilder.Entity<Student>().HasData(new List<Student>
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


            // Student sinifini ozellestirelim ;

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(n => n.StudentName).IsRequired();   // StudentName gereklidir.
                entity.Property(n =>n.Email).HasMaxLength(250);     // Maxsimim 250 karakter.
                entity.Property(n => n.Adres).IsRequired(false).HasMaxLength(500);    // Adres istege bagli.Maksimum 500 karakter.
                entity.Property(n => n.Email).IsRequired().HasMaxLength(250);         // Email gereklidir.Maksimum 250 karakter.
            });
        }
    }
}
