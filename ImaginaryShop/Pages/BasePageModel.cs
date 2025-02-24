using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImaginaryShop.Pages
{
    public class BasePageModel : PageModel
    {

		private string _currency;

		protected string Currency
		{
			get {

                
                if (Request.Cookies.TryGetValue("currency", out string currencyValue))
                {
                    _currency = currencyValue;
                }

                return _currency; 
			
			
			}
			set { _currency = value; }
		}

	}
}
