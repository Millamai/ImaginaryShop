using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace ImaginaryShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        [HttpPost]
        public IActionResult OnPostGeoLocation()
        {
            // Just to test that it actually gets called
            Debug.WriteLine("OnPostGeoLocation CALLED ####################################");

            return Page();
        }
    }
}
