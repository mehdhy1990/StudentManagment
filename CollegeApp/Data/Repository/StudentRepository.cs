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

        public async Task<Student> GetByIdAsync(int id)
        {
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
            var currentStudent = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == student.Id);
            if (currentStudent == null) throw new NullReferenceException("there is no student with this id");

            currentStudent.Id = student.Id;
            currentStudent.StudentName = student.StudentName;
            currentStudent.Address = student.Address;
            currentStudent.Email = student.Email;
            currentStudent.DOB = student.DOB;

            await _dbContext.SaveChangesAsync();
            return currentStudent.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var currentStudent = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
            if (currentStudent is null) throw new NullReferenceException("there is no student with this id");
            _dbContext.Students.Remove(currentStudent);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
