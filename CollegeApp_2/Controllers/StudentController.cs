using CollegeApp_2.Model;
using CollegeApp_2.Mylogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Xml.Linq;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {


        // LOGGER ; Kullanilabilir kaydedici yapalim ;
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }




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



            // LOGGER ;
            _logger.LogInformation("GetSudents method started");



            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Adres = s.Adres,
                Email = s.Email,
                Age = s.Age,
                //AdmissionDate = s.AdmissionDate,
                Password = s.Password,
                ConfirmPassword = s.ConfirmPassword
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

            // ----------------------------------------------- LGGER ;
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
            {
                // Kotu istek varsa bureya gunluk bilgilerinide ekleyelim
                _logger.LogWarning("Bad Reguest");
                return BadRequest();
            }


            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            // ----------------------------------------------  LGGER ;
            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
            {
                _logger.LogError("Student not fount with given Id");

                return NotFound($"The Student id {id} not fount ");
            }


            var studentDTO = new StudentDTO()
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Adres = student.Adres,
                Email = student.Email,
                Age = student.Age,
                AdmissionDate = student.AdmissionDate,
                Password = student.Password,
                ConfirmPassword = student.ConfirmPassword
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
                Age = student.Age,
                AdmissionDate = student.AdmissionDate,
                Password = student.Password,
                ConfirmPassword = student.ConfirmPassword,
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
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            //--------------------------------------------------------------------------------------------------------------------------

            /// -----------------   VALİDATİON  --------------------------------

            // Yukaridaki [ApiController] Yazmazsak Validationslari assagidaki gibi kontrol edebiliz

            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            // NOT  :  Yukariya [ApiController] yazdigimizdan ilgili DTO larda validationlari yaparak Controllerde birsey yapmamiza gerek yok



            //if (model.AdmissionDate <= DateTime.Now)
            //{
            /// 1. Model duruma hata mesajı eklemek ( Drectly adding error message modalstade )

            // ModelState.AddModelError("AdmissionDate error", "Admission date must be greater than or equal to todays date");
            // return BadRequest(ModelState);

            ///


            /// 2. Ozel metrigi kullanarak ozel niteligi kullanmaktir. ( Using custom  attribute)

            // Validators klasoru ekliiyoruz tum islimler orada

            /// 

            //}

            ///

            //--------------------------------------------------------------------------------------------------------------------------



            if (model == null)
                return BadRequest();

            int newId = CollegeRepository.Students.LastOrDefault().Id + 1;

            Student student = new Student()
            {
                Id = newId,
                StudentName = model.StudentName,
                Adres = model.Adres,
                Email = model.Email,
                Age = model.Age,
                AdmissionDate = model.AdmissionDate,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
            };

            CollegeRepository.Students.Add(student);

            model.Id = student.Id;

            // Status - 201
            // http://localhost:5164/api/Student/3
            // New student details
            return CreatedAtRoute("GetStudentsById", new { id = model.Id }, model); // Yeni olusturulan kayit  icin baglantiyi hazirlayacak
            return Ok(model);
        }


        [HttpPut]
        [Route("Update")]
        // api/Student/Update
        [ProducesResponseType(StatusCodes.Status204NoContent)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existringStudenr = CollegeRepository.Students.Where(s => s.Id == model.Id).FirstOrDefault();

            if (existringStudenr == null)
                return NotFound();

            existringStudenr.StudentName = model.StudentName;
            existringStudenr.Email = model.Email;
            existringStudenr.Adres = model.Adres;
            existringStudenr.Age = model.Age;
            existringStudenr.AdmissionDate = model.AdmissionDate;
            existringStudenr.Password = model.Password;
            existringStudenr.ConfirmPassword = model.ConfirmPassword;

            // 204 Kodu kayıt olundu icerik yok
            return NoContent(); // Kayit guncellendi ama dondurulecek iceri yok. yukarida Actiona <StudentDTO> yazmamiza gerek yok
        }

        // HttpPatch : Guncellemede tek bir yeri guncelliyorsak tum alanlati sunucuya gondermek yerine guncelledigimiz alani gondermemizi saglar
        // Assagidaki paketleri Nugetten kurmalisin :
        // 1 - Microsoft.AspNetCore.JsonPatch
        // 2 - Microsoft.AspNetCore.MVC.NewTonSoftJson

        // Sonra Program.cs de AddNewtonsoftJson ni ekledik

        [HttpPatch]
        [Route("{id:int} UpdatePartial")]
        // api/Student/1/UpdatePartial
        [ProducesResponseType(StatusCodes.Status204NoContent)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existringStudenr = CollegeRepository.Students.Where(s => s.Id == id).FirstOrDefault();

            if (existringStudenr == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existringStudenr.Id,
                StudentName = existringStudenr.StudentName,
                Adres = existringStudenr.Adres,
                Email = existringStudenr.Email,
                Age = existringStudenr.Age,
                AdmissionDate = existringStudenr.AdmissionDate,
                Password = existringStudenr.Password,
                ConfirmPassword = existringStudenr.ConfirmPassword

            };

            patchDocument.ApplyTo(studentDTO, ModelState); // ogrenci DTO suna uygulatiyoruz. Birseyler ters giderse ModelState ogrenicez

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            existringStudenr.StudentName = studentDTO.StudentName;
            existringStudenr.Email = studentDTO.Email;
            existringStudenr.Adres = studentDTO.Adres;
            existringStudenr.Age = studentDTO.Age;
            existringStudenr.AdmissionDate = studentDTO.AdmissionDate;
            existringStudenr.Password = studentDTO.Password;
            existringStudenr.ConfirmPassword = studentDTO.ConfirmPassword;

            // 204 - NoContent Kodu kayıt olundu icerik yok
            return NoContent(); // Kayit guncellendi ama dondurulecek iceri yok. yukarida Actiona <StudentDTO> yazmamiza gerek yok
        }



        [HttpDelete("Delete+{id:int}", Name = "DeleteStudent")]        // Name Routenin adi
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
