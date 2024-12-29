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
        [Route("All", Name = "GetStudents")]                        // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]            // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult<IEnumerable<Student>> GetStudents()
        {

            //var students = new List<StudentDTO>();

            //foreach (var item in CollegeRepository.Students)
            //{
            //    StudentDTO obj = new StudentDTO()
            //    {
            //        Id = item.Id,
            //        StudentName = item.StudentName,
            //        Adres = item.Adres,
            //        Email = item.Email,
            //    };
            //}

            // VEYAAAA

            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Adres = s.Adres,
                Email = s.Email,
            });

            // Ok - 200 - Success
            return Ok(students);

        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentsById")]              // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]            // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult<Student> GetStudentsById(int id)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
                return BadRequest();

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {id} not fount ");

            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Adres = student.Adres,
                Email = student.Email,
            };


            // Ok - 200 - Success
            return Ok(studentDTO);

        }

        [HttpGet("{name}", Name = "GetStudentsByName")]         // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult<Student> GetStudentsByName(string name)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (string.IsNullOrEmpty(name))
                return BadRequest();


            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {name} not fount ");

            var studentDTO = new StudentDTO()
            {

                Id = student.Id,
                StudentName = student.StudentName,
                Adres = student.Adres,
                Email = student.Email,
            };


            // Ok - 200 - Success
            return Ok(studentDTO);

        }


        [HttpPost]
        [Route("Create")]        // Name Routenin adi
        // api/Student/Create
        [ProducesResponseType(StatusCodes.Status201Created)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult<StudentDTO> CreateStudent([FromBody]StudentDTO model)
        {
            if (model == null)
                return BadRequest();

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1 ;

            Student student = new Student()
            {
                Id =newId,
                StudentName = model.StudentName,
                Adres = model.Adres,
                Email = model.Email,
            };

            CollegeRepository.Students.Add(student);

            model.Id = student.Id;

            // Status - 201
            // http://localhost:5164/api/Student/3
            // New student details
            return CreatedAtRoute("GetStudentsById", new { id=model.Id },model ); // Yeni olusturulan kayit  icin baglantiyi hazirlayacak
            return Ok(model);
        }


        [HttpDelete("{id:int}", Name = "DeleteStudent")]        // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
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
