using ImaginaryShop.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImaginaryShop.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassOMeterController : ControllerBase
    {

        [HttpPost("score")]
        public IActionResult GetPasswordScore([FromBody] PasswordRequest request)
        {
            
            Debug.WriteLine(request.Password);
            PassOMeter p = new PassOMeter();

            return Ok(new { score = p.Validate(request.Password) }) ;
        }
    }

    public class PasswordRequest
    {
        public string Password { get; set; }
    }
}
