using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TokenDemoG1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public ActionResult Get() 
        {
            return Ok(new List<string> { "testAuth1", "testAuth2" });
        }

        [HttpGet]
        [Authorize("CEO")]
        [Route("GetAll")]
        public ActionResult GetAlForEngineer()
        {
            return Ok(new List<string> { "testAuth3", "testAuth4" });
        }
    }
}
