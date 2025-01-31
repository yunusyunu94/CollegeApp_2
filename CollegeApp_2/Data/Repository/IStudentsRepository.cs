namespace CollegeApp_2.Data.Repository
{
    public interface IStudentsRepository : ICollageRepository<Student>
    {
        Task <List<Student>> GetStudentsByFeeStatusAsync(int feeStatus); // Ogrenci ucret durumuna gore
    }
}
