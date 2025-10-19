namespace CollegeApp.Data.Repository
{
    public interface IStudentRepository
    {
       Task<List<Student>>  GetStudentByFeesAsync(int fee);


    }
}
