namespace ImaginaryShop.Model
{
    public class User
    {
        public int UserId { get; set; }          // Unikt ID for hver bruger
        public string UserName { get; set; }     // Brugernavn (kan være email)
        public string Email { get; set; }        // Brugerens email
        public string PasswordHash { get; set; } // Hashed adgangskode
        public string FullName { get; set; }     // Brugeren fulde navn
        public string Role { get; set; }         // Brugerens rolle (f.eks. "Admin", "User")
        public DateTime CreatedAt { get; set; }  // Oprettelsestidspunkt
        public DateTime? LastLogin { get; set; } // Sidste login tidspunkt
    }
}
