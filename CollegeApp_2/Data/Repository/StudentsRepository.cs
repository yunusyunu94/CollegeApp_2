
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data.Repository
{
    public class StudentsRepository : CollageRepository<Student>, IStudentsRepository
    {
        private readonly CollegeDBContext _dbContext;
        public StudentsRepository(CollegeDBContext dbContext) : base(dbContext) // Parametreyi taban sinifina " BASE " yoluyla gecirdik
        {
            _dbContext = dbContext;
        }


        // Bu " StudentsRepository " sınıfa ogrencilerle ilgili ozel yontemleri yazabiliriz or ; ogrenci ucretleri, ogrenci durumları gibi


        public Task<List<Student>> GetStudentsByFeeStatusAsync(int feeStatus)
        {
            // Write code to return students having fee status pending
            return null;
        }

        


    }
}
