using System.Linq.Expressions;
using AutoMapper;
using CollegeApp_2.Data;
using CollegeApp_2.Data.Repository;
using CollegeApp_2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(policyName: "AllowOnlyLocalhost")]  // Program.cs de tanimladigimiz CORS yontemlerinden hangisini kullanicaksak controllere bu sekilde uyguluyoruz
    [Authorize(Roles ="Superadmin,Admin")]

    public class StudentController : ControllerBase
    {


        // LOGGER ; Kullanilabilir kaydedici yapalim ;
        private readonly ILogger<StudentController> _logger;

        // private readonly CollegeDBContext _dbContext; // Veritabanini artik CollageRepository kullaniyoruz

        // private readonly IStudentsRepository _studentsRepository;  // Bunun depo yerine CollageRepository tum depolar icin ortak olusturduk asssagidakini uygulucaz ;
        // private readonly ICollageRepository<Student> _studentRepository; // Artik " CollageRepository " ortak depo oldugundan hangi tablonun controllunda calisiyorsak
                                                                            // ona ozgu depo kullanicaz assagıdaki gibi ;
        private readonly IStudentsRepository _studentRepository;

        private readonly IMapper _mapper; //  AutoMapper 

        public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentsRepository studentRepository)
        {
            _logger = logger;

            //_dbContext = dBContext;
            _studentRepository = studentRepository;

            _mapper = mapper;

        }




        [HttpGet]
        [Route("All", Name = "GetStudents")]                        // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]            // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        [AllowAnonymous]  // jwt token kisitlama yok demek
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {


            // LOGGER ;
            _logger.LogInformation("GetSudents method started");


            var students = await _studentRepository.GetAllAsync();


            //  AutoMapper ;
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students);

            // Ok - 200 - Success
            return Ok(studentDTOData);

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


            var student = await _studentRepository.GetAsync(student => student.Id == id);

            // ----------------------------------------------  LGGER ;
            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
            {
                _logger.LogError("Student not fount with given Id");

                return NotFound($"The Student id {id} not fount ");
            }

            //  AutoMapper ;
            var studentDTO = _mapper.Map<StudentDTO>(student);


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


            var student = await _studentRepository.GetAsync(student => student.StudentName.ToLower().Contains(name.ToLower())); // Contains kismi eslesme ve ToLower kucuk/buyuk harflere duyarli

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {name} not fount ");


            //  AutoMapper ;
            var studentDTO = _mapper.Map<StudentDTO>(student);

            // Ok - 200 - Success
            return Ok(studentDTO);

        }


        [HttpPost]
        [Route("Create")]        // Name Routenin adi
        // api/Student/Create
        [ProducesResponseType(StatusCodes.Status201Created)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO dto)
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



            if (dto == null)
                return BadRequest();

            //  AutoMapper ;
            Student student = _mapper.Map<Student>(dto);

            var studentAfterCreation = await _studentRepository.CreateAsync(student);

            dto.Id = studentAfterCreation.Id;

            // Status - 201
            // http://localhost:5164/api/Student/3
            // New student details
            return CreatedAtRoute("GetStudentsById", new { id = dto.Id }, dto); // Yeni olusturulan kayit  icin baglantiyi hazirlayacak

        }


        [HttpPut]
        [Route("Update")]
        // api/Student/Update
        [ProducesResponseType(StatusCodes.Status204NoContent)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <= 0)
                BadRequest();

            var existringStudenr = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);


            if (existringStudenr == null)
                return NotFound();


            //  AutoMapper ;
            var newRecort = _mapper.Map<Student>(dto);

            await _studentRepository.UpdateAsync(newRecort);

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
        public async Task<ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
                BadRequest();

            var existringStudenr = await _studentRepository.GetAsync(student => student.Id == id, true);

            if (existringStudenr == null)
                return NotFound();


            //  AutoMapper ;
            var studentDTO = _mapper.Map<StudentDTO>(existringStudenr);


            patchDocument.ApplyTo(studentDTO, ModelState); // ogrenci DTO suna uygulatiyoruz. Birseyler ters giderse ModelState ogrenicez

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            existringStudenr = _mapper.Map<Student>(studentDTO);

            await _studentRepository.UpdateAsync(existringStudenr);


            // 204 - NoContent Kodu kayıt olundu icerik yok
            return NoContent(); // Kayit guncellendi ama dondurulecek iceri yok. yukarida Actiona <StudentDTO> yazmamiza gerek yok
        }



        [HttpDelete("Delete+{id:int}", Name = "DeleteStudent")]        // Name Routenin adi
        [ProducesResponseType(StatusCodes.Status200OK)]        // Hata kodlarin kullanicilar tarafindan okunabilmesi 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // Sunucu hatasi varsa
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            // BadRequest - 400 - BadRequest - Ciend Error
            if (id <= 0)
                return BadRequest();


            var student = await _studentRepository.GetAsync(student => student.Id == id);

            // NotFound - 404 - NotFound - Ciend Error
            if (student == null)
                return NotFound($"The Student id {id} not fount ");

            await _studentRepository.DeleteAsync(student);

            // Ok - 200 - Success
            return Ok(true);
        }
    }
}
