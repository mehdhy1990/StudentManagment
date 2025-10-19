using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //transforming to Datetime
            //CreateMap<StudentDTO,Student>().ReverseMap().AddTransform<DateTime>(n=>Convert.ToDateTime(n));
            CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Address,
                    opt => opt
                        .MapFrom(x => string.IsNullOrEmpty(x.Address) ? "there is no address" : x.Address));
        }
    }
}
