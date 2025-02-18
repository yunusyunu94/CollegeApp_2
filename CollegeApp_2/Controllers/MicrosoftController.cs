using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(policyName: "AllowOnlyMicrosoft")]  // Program.cs de tanimladigimiz CORS yontemlerinden hangisini kullanicaksak controllere bu sekilde uyguluyoruz
    public class MicrosoftController : ControllerBase
    {
    }
}
