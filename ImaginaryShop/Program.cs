using ImaginaryShop.Model.Repos;
using ImaginaryShop.Model.Services;

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

            app.Run();
        }
    }
}
