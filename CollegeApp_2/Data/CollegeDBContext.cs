using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data
{
    public class CollegeDBContext : DbContext
    {
        DbSet<Student> Students { get; set; }
    }
}
