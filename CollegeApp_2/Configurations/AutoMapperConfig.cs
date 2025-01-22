using AutoMapper;
using CollegeApp_2.Data;
using CollegeApp_2.Model;

namespace CollegeApp_2.Configurations
{

    // Nugetten yukleyecegimiz kutuphane ; " AutoMapper " 
    // AutoMapper icin " Profile " den miras almaliyiz
    public class AutoMapperConfig : Profile
    {
         public AutoMapperConfig()
        {
            //CreateMap<Student, StudentDTO>(); // Student verileri StudentDTO ya kopyaliyoruz
            //CreateMap<StudentDTO, Student>(); // StudentDTO verileri Student de kopyaliyoruz

            // VEA

            CreateMap<Student, StudentDTO>().ReverseMap();  // Yukaridaki 2 kodla ayni kisaltmasi

            // Daha sonra Program.cs 
        }
    }
}
