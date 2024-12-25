using CollegeApp_2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Student> GetStudents()
        {
            return new List<Student>(){
                new Student
                {
                    Id = 1,
                    StudentName = "Student 1",
                    Email ="Studentemail1@hotmail.com",
                    Adres = "Hyd, INDIA"
                      
                },
                new Student
                {
                    Id = 1,
                    StudentName = "Student 2",
                    Email = "Studentemail1@hotmai2.com",
                    Adres = "Banglore, INDIA"

                }
            };
        }
    }
}
