using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Json;

namespace ImaginaryShop.Controllers
{


    /// <summary>
    /// Controller, der håndterer login- og logout-funktionalitet for brugere.
    /// </summary>
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialiserer en ny instans af <see cref="LoginController"/> med de nødvendige afhængigheder.
        /// </summary>
        /// <param name="configuration">Applikationens konfigurationsinstillinger.</param>
        /// <param name="userRepository">Repository til håndtering af brugerdata.</param>
        public LoginController(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Håndterer loginanmodninger og validerer brugerens legitimationsoplysninger.
        /// </summary>
        /// <param name="loginModel">Modellen, der indeholder brugerens loginoplysninger.</param>
        /// <returns>
        /// Returnerer en HTTP-statuskode afhængigt af loginprocessen:
        /// - 200 OK med en redirect-URL, hvis login er succesfuldt.
        /// - 400 Bad Request, hvis input ikke er validt.
        /// - 401 Unauthorized, hvis brugernavn eller adgangskode er forkert.
        /// </returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Validerer først inputtet for at sikre, at det overholder modelvalideringsreglerne
            if (!ModelState.IsValid)
            {
                // Henter den første fejlbesked fra modelstate
                string error = ModelState.Values.SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage)
                                                .FirstOrDefault();
                return BadRequest(new { message = error });
            }

            // Henter brugeren fra databasen baseret på det indtastede brugernavn
            User u = _userRepository.GetUserByUserName(loginModel.Username);


            // Tjekker om brugeren eksisterer og verificerer adgangskoden ved hjælp af Argon2
            if ((u != null) && (Argon2.Verify(u.PasswordHash, loginModel.Password)))
            {

                // Opretter en liste af claims (påstande) om brugeren
                List<Claim> claims = new List<Claim>
        {
            // Standard claim, der gemmer brugerens fulde navn
            new Claim(ClaimTypes.Name, u.FullName),
            
            // Brugerdefineret claim, der gemmer brugernavn
            new Claim("Username", u.UserName)
        };

                // Opretter en ny identitet og principal baseret på claims
                var identity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(identity);

                // Autentificerer brugeren ved at oprette en cookie
                await HttpContext.SignInAsync(principal);

                // Returnerer en redirect-URL til den side, brugeren kom fra
                return Ok(new { redirectUrl = Request.Headers["Referer"].ToString() });
            }
            else
            {
                // Returnerer en 401 Unauthorized, hvis loginoplysningerne er forkerte
                return Unauthorized(new { message = "Der findes ikke en bruger med dette brugernavn eller password" });
            }
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
                HttpContext.Session.Clear(); // Tømmer sessionen
                foreach (var cookie in Request.Cookies)
                {
                    if (cookie.Key.StartsWith(".AspNetCore"))
                    {
                        Response.Cookies.Delete(cookie.Key);
                    }
                }
            }

            return Ok(new { redirectUrl = "/Index" });
        }
    }
}
