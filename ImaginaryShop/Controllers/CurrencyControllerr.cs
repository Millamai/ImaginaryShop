using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImaginaryShop.Controllers
{
    [Route("currency")]
    public class CurrencyControllerr : Controller
    {
        [HttpGet("set/{currency}")]
        public IActionResult SetCurrency(string currency)
        {

            if (!string.IsNullOrEmpty(currency))
            {
                // Sæt en cookie, der varer i 30 dage
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30),
                    HttpOnly = true, // Beskytter mod XSS ASVS 
                    Secure = true,   // Kun via HTTPS ASVS 
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("currency", currency, options);
            }

            //TODO input validering her....
            return Redirect(Request.Headers["Referer"].ToString()); // Gå tilbage til hvorfra vi kom
        }
    }
}
