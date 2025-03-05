namespace ImaginaryShop.Model
{
    public class User
    {


       public enum UserRole
        {
            Admin = 0,
            Customer = 1
        }
        public int UserId { get; set; }          // Unikt ID for hver bruger
        public string UserName { get; set; }     // Brugernavn (kan være email)
        public string Email { get; set; }        // Brugerens email
        public string PasswordHash { get; set; } // Hashed adgangskode
        public string FullName { get; set; }     // Brugeren fulde navn
        public UserRole Role { get; set; }         // Brugerens rolle (f.eks. "Admin", "User")
        public DateTime CreatedAt { get; set; }  // Oprettelsestidspunkt
        public DateTime? LastLogin { get; set; } // Sidste login tidspunkt
    }
}
