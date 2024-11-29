using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace UserService.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public UserController()
        {


        }

        [HttpGet]
        [Route("HelloWorld")]
        public async Task<IActionResult> HelloWorld()
        {
            return Ok("HOLA MUNDO");
        }


  
    }
}
