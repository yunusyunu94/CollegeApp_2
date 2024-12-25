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
            return CollegeRepository.Students;
            
        }
        [HttpGet("{id:int}")]
        public Student GetStudentsById(int id)
        {
            return CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            
        }
    }
}
