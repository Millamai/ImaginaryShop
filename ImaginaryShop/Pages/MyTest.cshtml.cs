using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace ImaginaryShop.Pages
{
    public class MyTestModel : PageModel
    {
        public IActionResult OnGet()
        {

            Debug.WriteLine("GET");
            return Page();
        }
    }
}
