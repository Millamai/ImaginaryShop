using ImaginaryShop.Model;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImaginaryShop.Controllers
{

    //Controller for handling login functionality


    [Route("Login")]
    public class LoginController : Controller
    {

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // TODO: Input validering 
            //Find brugeren i databasen

            UserRepository ur = new UserRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");
            User u = ur.GetUserByUserName(loginModel.Username);



            if ((u != null) && (Argon2.Verify(u.PasswordHash, loginModel.Password)))
            {
                //Redirect til der hvor brugeren kommer fra
                return Ok(new { redirectUrl = Request.Headers["Referer"].ToString() });
            }
            else
                return Unauthorized(new { message = "Invalid username or password" });




        }
    }
}
