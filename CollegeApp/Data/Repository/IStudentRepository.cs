namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository : ICollegeRepository<Student>
    {
       Task<List<Student>>  GetStudentByFeesAsync(int fee);


    }
}
