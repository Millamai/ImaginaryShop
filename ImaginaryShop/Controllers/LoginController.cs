using ImaginaryShop.Model;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;

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
              //  Når brugeren logger ind med det rigtige brugernavn og password,
              //  opretter vi en simpel cookie ved at bruge SignInAsync og gemmer
              //  brugerens oplysninger(f.eks.brugernavn) i claims.
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name, u.FullName));
                claims.Add(new Claim("Username", u.UserName));
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Sæt cookie for den autentificerede bruger
                HttpContext.SignInAsync(principal);

                //Redirect til der hvor brugeren kommer fra
                return Ok(new { redirectUrl = Request.Headers["Referer"].ToString() });
            }
            else
                return Unauthorized(new { message = "Invalid username or password" });




        }
    }
}
