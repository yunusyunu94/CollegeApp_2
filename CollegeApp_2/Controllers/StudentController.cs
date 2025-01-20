using CollegeApp_2.Data;
using CollegeApp_2.Model;
using CollegeApp_2.Mylogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {


        // LOGGER ; Kullanilabilir kaydedici yapalim ;
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dBContext)
        {
            _logger = logger;
            _dbContext = dBContext;
        }




        [HttpGet]
        [Route("All", Name = "GetStudents")]                        // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]            // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {


            // LOGGER ;
            _logger.LogInformation("GetSudents method started");



            var students = await _dbContext.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Adres = s.Adres,
                Email = s.Email,
                DOB = s.DOB,

            }).ToListAsync();

            // Ok - 200 - Success
            return Ok(students);

        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentsById")]              // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]            // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult<Student>> GetStudentsByIdAsync(int id)
        {

            // ----------------------------------------------- LGGER ;
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
            {
                // Kotu istek varsa bureya gunluk bilgilerinide ekleyelim
                _logger.LogWarning("Bad Reguest");
                return BadRequest();
            }


            var student = await _dbContext.Students.Where(n => n.Id == id).FirstOrDefaultAsync();

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
                DOB = student.DOB,
            };


            // Ok - 200 - Success
            return Ok(studentDTO);

        }

        [HttpGet("{name}", Name = "GetStudentsByName")]         // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult<Student>> GetStudentsByNameAsync(string name)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (string.IsNullOrEmpty(name))
                return BadRequest();


            var student = await _dbContext.Students.Where(n => n.StudentName == name).FirstOrDefaultAsync();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {name} not fount ");

            var studentDTO = new StudentDTO()
            {

                Id = student.Id,
                StudentName = student.StudentName,
                Adres = student.Adres,
                Email = student.Email,
                DOB = student.DOB,
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
        public async Task <ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO model)
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


            Student student = new Student()
            {
                StudentName = model.StudentName,
                Adres = model.Adres,
                Email = model.Email,
                DOB = model.DOB,
            };

            await _dbContext.Students.AddAsync(student);

            await _dbContext.SaveChangesAsync(); // Vari tabanina degisiklikleri kaydediyoruz

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
        public async Task <ActionResult> UpdateStudentAsync([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                BadRequest();

            var existringStudenr = await _dbContext.Students.Where(s => s.Id == model.Id).FirstOrDefaultAsync();

            if (existringStudenr == null)
                return NotFound();

            existringStudenr.StudentName = model.StudentName;
            existringStudenr.Email = model.Email;
            existringStudenr.Adres = model.Adres;
            existringStudenr.DOB = model.DOB;
            //existringStudenr.DOB = Convert.ToDateTime(model.DOB);

            await _dbContext.SaveChangesAsync(); // Vari tabanina degisiklikleri kaydediyoruz

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
        public async Task <ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existringStudenr = await _dbContext.Students.Where(s => s.Id == id).FirstOrDefaultAsync();

            if (existringStudenr == null)
                return NotFound();

            var studentDTO = new StudentDTO
            {
                Id = existringStudenr.Id,
                StudentName = existringStudenr.StudentName,
                Adres = existringStudenr.Adres,
                Email = existringStudenr.Email,
                DOB = existringStudenr.DOB,

            };

            patchDocument.ApplyTo(studentDTO, ModelState); // ogrenci DTO suna uygulatiyoruz. Birseyler ters giderse ModelState ogrenicez

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            existringStudenr.StudentName = studentDTO.StudentName;
            existringStudenr.Email = studentDTO.Email;
            existringStudenr.Adres = studentDTO.Adres;
            existringStudenr.DOB = studentDTO.DOB;

            await _dbContext.SaveChangesAsync(); // Vari tabanina degisiklikleri kaydediyoruz

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


            var student = _dbContext.Students.Where(n => n.Id == id).FirstOrDefault();

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {id} not fount ");

            _dbContext.Students.Remove(student);

            _dbContext.SaveChanges(); // Vari tabanina degisiklikleri kaydediyoruz

            // Ok - 200 - Success
            return Ok(true);
        }
    }
}
