namespace CollegeApp_2.Data.Repository
{
    public interface IStudentsRepository
    {
        Task <List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
         Task <Student> GetByNameAsync(string name);
         Task <int> CreateAsync(Student student);
        Task<int> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
