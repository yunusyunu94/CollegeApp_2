using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
                
        }
        DbSet<Student> Students { get; set; }
    }
}
