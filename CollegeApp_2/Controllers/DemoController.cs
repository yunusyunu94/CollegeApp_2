using CollegeApp_2.Mylogging;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(policyName: "AllowOnlyGoogle")]  // Program.cs de tanimladigimiz CORS yontemlerinden hangisini kullanicaksak controllere bu sekilde uyguluyoruz
    public class DemoController : ControllerBase
    {
        /// -------------------- 1. Strongly coumpled/Tightlt coumpled -------------------

        //private readonly IMyLogger _myLogger;

        //public DemoController()
        //{
        //    //_myLogger = new LogToFile();
        //    _myLogger = new LogToDb();

        //}

        //[HttpGet]
        //public ActionResult Index() 
        //{
        //    _myLogger.Log("Index method started");
        //    return Ok();

        //}


        /// ---------------------- 2. Loosely coumpled ------------------------

        /// Program.cs de Container olusturup bagımliligi minimize etmeliyiz

        //private readonly IMyLogger _myLogger;

        //public DemoController(IMyLogger myLogger)
        //{
        //    //_myLogger = new LogToFile();
        //    _myLogger = myLogger;

        //}

        //[HttpGet]
        //public ActionResult Index()
        //{
        //    _myLogger.Log("Index method started");
        //    return Ok();

        //}

        // --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// ---------------   LOGGER SEVİYELERİNİ DENİCEZ  -----------------------------------------------------------


        private readonly ILogger<StudentController> _logger;

        public DemoController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Index()
        {
            _logger.LogTrace ("Log message from trace method");
            _logger.LogDebug("Log message from Debug method");
            _logger.LogInformation("Log message from Information method");
            _logger.LogWarning("Log message from Warning method");
            _logger.LogError("Log message from Error method");
            _logger.LogCritical("Log message from Critical method");  
            return Ok();

        }










    }
}
