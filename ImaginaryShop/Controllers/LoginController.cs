using ImaginaryShop.Model;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            // TODO: Input validering 
            //Find brugeren i databasen
            //TODO : Change conn string

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




        /// <summary>
        /// Logger brugeren ud af systemet og sletter autentificeringscookien.
        /// </summary>
        /// <returns>Returnerer en JSON-besked med en redirect-url.</returns>
        /// <response code="200">Brugeren er logget ud, og cookien er slettet.</response>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            // Udfør sign-out med cookie authentication
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Slet autentificeringscookien eksplicit
            if (!string.IsNullOrEmpty(_configuration["CookieSettings:AuthCookie"]))
            {
                Response.Cookies.Delete(_configuration["CookieSettings:AuthCookie"]);
            }

            return Ok(new { redirectUrl = "/Index" });
        }
    }
}
