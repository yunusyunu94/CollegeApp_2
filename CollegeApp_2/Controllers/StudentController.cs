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
        [Route("All", Name = "GetStudents")] // Name Routenin adi
        public IEnumerable<Student> GetStudents()
        {
            return CollegeRepository.Students;

        }

        [HttpGet]
        [Route("{id}", Name = "GetStudentsById")]
        public Student GetStudentsById(int id)
        {
            return CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

        }

        [HttpGet("{name}", Name = "GetStudentsByName")]
        public Student GetStudentsByName(string name)
        {
            return CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        public bool DeleteStudent(int id)
        {
            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            CollegeRepository.Students.Remove(student);



            return true;
        }
    }
}
