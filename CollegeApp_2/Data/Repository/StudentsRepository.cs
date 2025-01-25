
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data.Repository
{
    public class StudentsRepository : IStudentsRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentsRepository(CollegeDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }


        public async Task<Student> GetByIdAsync(int id,bool useNoTracking = false)
        {
            if (useNoTracking)
            return await _dbContext.Students.AsNoTracking().Where(student => student.Id == id).FirstOrDefaultAsync();
            else 
                return await _dbContext.Students.Where(student => student.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbContext.Students.Where(Student => Student.StudentName.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
            // Buyuktur/Kucuktur harflere duyarli olsun, Equals esittire icin biz Contains yapicaz yani icerir dicez
        }


        public async Task<int> CreateAsync(Student student)
        {
            _dbContext.Students.Add(student);

            await _dbContext.SaveChangesAsync();

            return student.Id;
        }


        public async Task <int> UpdateAsync(Student student)
        {
            _dbContext.Update(student);

            await _dbContext.SaveChangesAsync();
            return student.Id;
        
        }

        public async Task <bool> DeleteAsync(Student student)
        {

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return true;

        }


        
    }
}
