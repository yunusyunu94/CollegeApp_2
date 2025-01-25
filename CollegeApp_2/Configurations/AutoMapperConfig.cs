using AutoMapper;
using CollegeApp_2.Data;
using CollegeApp_2.Model;
using Microsoft.IdentityModel.Tokens;

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

            // CreateMap<Student, StudentDTO>().ReverseMap();  // Yukaridaki 2 kodla ayni kisaltmasi

            // Daha sonra Program.cs 

            // DTO ile normal sinif arasinda isim farklilik var ise NULL doner bunun icin StudentDTO da StudentName yi Name yaptik Student sinifinda StudentName olarak yazdik
            // OtoMapperde yapmak icin ;
            // CreateMap<StudentDTO, Student>().ForMember(n => n.StudentName, opt => opt.MapFrom(x => x.Name )).ReverseMap();



            // Eger herhangi bir eslesmeyi yoksaymak istiyorsak ;
            // CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.Name, opt => opt.Ignore()); // Burada StudentNamei yoksayacak ve eslestirme yapmayacaktir.



            // Eger bir  degeri NULL donuyorsan NULL yerine anlamli birsey yazabiliriz bunun icin ;
            // CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n => string.IsNullOrEmpty(n) ? "No address fount " : n); // Tum alanlar icin gecerlidir.
            // Tek alana ayri mesaj gondermek icin ;
             CreateMap<StudentDTO, Student>().ReverseMap()
                .ForMember(n => n.Adres, opt => opt.MapFrom(n => string.IsNullOrEmpty(n.Adres) ? "No address fount " : n.Adres));

        }
    }
}
