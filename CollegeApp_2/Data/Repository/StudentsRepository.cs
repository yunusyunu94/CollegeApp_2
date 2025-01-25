
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


        public async Task<Student> GetByIdAsync(int id)
        {
            return await _dbContext.Students.Where(Student => Student.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dbContext.Students.Where(Student => Student.StudentName.ToLower().Equals(name.ToLower())).FirstOrDefaultAsync();
            // Buyuktur/Kucuktur harflere duyarli olsun
        }


        public async Task<int> CreateAsync(Student student)
        {
            _dbContext.Students.Add(student);

            await _dbContext.SaveChangesAsync();

            return student.Id;
        }


        public async Task <int> UpdateAsync(Student student)
        {
            var studentToUpdate = await _dbContext.Students.Where(student => student.Id == student.Id).FirstOrDefaultAsync();

            if (studentToUpdate == null)
                throw new ArgumentException($"No student fount with id: {student.Id}");

            studentToUpdate.StudentName = student.StudentName;
            studentToUpdate.Email = student.Email;
            studentToUpdate.Adres = student.Adres;
            studentToUpdate.DOB = student.DOB;

            await _dbContext.SaveChangesAsync();
            return student.Id;
        
        }

        public async Task <bool> DeleteAsync(int id)
        {
            var studentToDelete = await _dbContext.Students.Where(student => student.Id == student.Id).FirstOrDefaultAsync();

            if (studentToDelete == null)
                throw new ArgumentException($"No student fount with id: {id}");

            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();

            return true;

        }


        
    }
}
