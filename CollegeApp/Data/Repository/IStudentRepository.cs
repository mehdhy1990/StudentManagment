namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByNameAsync(string name);
        Task<int> CreateAynce(Student student);
        Task<int> UpdateAsync(Student student);
        Task<bool> Delete(int id);


    }
}
