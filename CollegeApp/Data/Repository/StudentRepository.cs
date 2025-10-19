using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDbContext _dbContext;

        public StudentRepository(CollegeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dbContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id,bool useNoTracking=false)
        {
            if (useNoTracking is true) return await _dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return (await _dbContext.Students.FirstOrDefaultAsync(x => x.StudentName.ToLower().Equals(name.ToLower())));
        }

        public async Task<int> CreateAynce(Student student)
        {
            _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> UpdateAsync(Student student)
        {
            _dbContext.Update(student);

            await _dbContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
           
            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
