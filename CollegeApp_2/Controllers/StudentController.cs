using CollegeApp_2.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetStudents")] // Name Routenin adi
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            // Ok - 200 - Success
            return Ok(CollegeRepository.Students);

        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentsById")]
        public ActionResult<Student> GetStudentsById(int id)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {id} not fount ");

            // Ok - 200 - Success
            return Ok(student);

        }

        [HttpGet("{name}", Name = "GetStudentsByName")]
        public ActionResult<Student> GetStudentsByName(string name)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (string.IsNullOrEmpty(name))
                return BadRequest();


            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {name} not fount ");

            // Ok - 200 - Success
            return Ok(student);

        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        public ActionResult<bool> DeleteStudent(int id)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
                return BadRequest();


            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {id} not fount ");

            CollegeRepository.Students.Remove(student);

            // Ok - 200 - Success
            return Ok(true);
        }
    }
}
