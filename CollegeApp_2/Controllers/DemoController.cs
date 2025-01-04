using CollegeApp_2.Mylogging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        // 1. Strongly coumpled/Tightlt coumpled 

        private readonly IMyLogger _myLogger;

        public DemoController()
        {
            //_myLogger = new LogToFile();
            _myLogger = new LogToDb();
        
        }

        [HttpGet]
        public ActionResult Index() 
        {
            _myLogger.Log("Index method started");
            return Ok();
        
        }

















        // 1. Loosely coumpled
    }
}
