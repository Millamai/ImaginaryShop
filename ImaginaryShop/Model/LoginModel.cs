using System.ComponentModel.DataAnnotations;

namespace ImaginaryShop.Model
{/// <summary>
 /// Model til brugerlogin, der indeholder brugernavn og adgangskode med validering.
 /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Brugernavnet, som brugeren indtaster ved login.
        /// </summary>
        /// <remarks>
        /// - Skal være mellem 3 og 50 tegn.  
        /// - Kun bogstaver, tal og underscores er tilladt.  
        /// - Feltet er obligatorisk.
        /// </remarks>
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        [StringLength(50, MinimumLength =6, ErrorMessage = "Brugernavn skal være mellem 6 og 50 tegn")]
        [RegularExpression(@"^[^<>&%]+$", ErrorMessage = "Brugernavn må ikke indeholde <, >, & eller %")]
        public string Username { get; set; }

        /// <summary>
        /// Adgangskoden, som brugeren indtaster ved login.
        /// </summary>
        /// <remarks>
        /// - Skal være mindst 6 tegn lang.  
        /// - Feltet er obligatorisk.
        /// </remarks>
        [Required(ErrorMessage = "Adgangskode er påkrævet")]
        [StringLength(128, MinimumLength = 12, ErrorMessage = "Adgangskode skal være mindst 12 tegn langt")]
        public string Password { get; set; }
    }
}
