using CollegeApp_2.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
                
        }
       public DbSet<Student> Students { get; set; }
       public DbSet<Department> Departments { get; set; }


       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo 1
            modelBuilder.ApplyConfiguration(new StudentConfig()); // StudentConfig sinifini burda tanimliyoruz.Her tablo icin ayri ayri tanimliyoruz
            modelBuilder.ApplyConfiguration(new DepartmentConfig()); // Department sinifini burda tanimliyoruz.Her tablo icin ayri ayri tanimliyoruz



        }
    }
}
