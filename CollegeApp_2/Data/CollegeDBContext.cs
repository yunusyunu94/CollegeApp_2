using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
                
        }
        DbSet<Student> Students { get; set; }


        // Vari tabanina elle veri ekleme;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}
