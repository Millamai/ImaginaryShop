using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using ImaginaryShop.Model.Services;
using Isopoh.Cryptography.Argon2;


namespace ImaginaryShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<CategoryService>();

            builder.Services.AddDistributedMemoryCache(); // Bruger hukommelsen til at lagre sessiondata
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout for session
                options.Cookie.HttpOnly = true; // Forhindrer adgang til sessionen fra JavaScript
            });

            builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //Så cookies kan tilgås i cshtml
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();
            app.UseSession();  // Dette skal være før app.UseEndpoints()


            // Registrer IHttpContextAccessor


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }



            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();
            app.MapControllers();

         //      Seed();
            app.Run();
        }


        private static void Seed()
        {
            User u = new User();
            u.UserName = "Test2";
            u.FullName = "Camilla";
            u.Email = "Test2";
            u.Role = "Customer";

            string pass = "123";
          string hashedpassword = Argon2.Hash(pass);
            u.PasswordHash = hashedpassword;

            UserRepository ur = new UserRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");
            ur.CreateUser(u);



        }
    }
}
