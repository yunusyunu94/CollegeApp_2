using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp_2.Data.Repository
{
    public class CollageRepository<T> : ICollageRepository<T> where T : class
    {
        private readonly CollegeDBContext _dbContext;
        private DbSet<T> _dbSet;
        public CollageRepository(CollegeDBContext dbContext) 
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>(); // Bubizim tablomuz T ye gelecek verileri bu tablo ustunden yapicaz
        }


        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);

            await _dbContext.SaveChangesAsync();

            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {

            _dbSet.Remove(dbRecord);
            await _dbContext.SaveChangesAsync();

            return true;

        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
               // return await _dbSet.AsNoTracking().Where(Student => Student.Id == id).FirstOrDefaultAsync();
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            else
                // return await _dbSet.Where(Student => Student.Id == id).FirstOrDefaultAsync();
                 return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }


        // ASSAGIDAGİ YONTEMİ KALDİRDİK " GetByIdAsync " AYNİ GOREVİGORUYOR
        //public async Task<T> GetByNameAsync(Expression<Func<T, bool>> filter)
        //{
        //    // return await _dbSet.Where(Student => Student.StudentName.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        //    // Buyuktur/Kucuktur harflere duyarli olsun, Equals esittire icin biz Contains yapicaz yani icerir dicez

        //    return await _dbSet.Where(filter).FirstOrDefaultAsync();
        //}

        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);

            await _dbContext.SaveChangesAsync();
            return dbRecord;

        }

    }
}
