using ImaginaryShop.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;

namespace ImaginaryShop.Pages
{
    public class ShoppingPageModel : BasePageModel
    {
        /// <summary>
        /// Henter indkøbskurven på sessionen, hvis den eksisterer ellers oprettes en ny
        /// </summary>
        /// <param name="basket">Kurven der gemmes</param>
        protected ShoppingBasket GetShoppingBasket()
        {
            ShoppingBasket sb;

            var basket = HttpContext.Session.GetString("Basket");

            if (basket == null)
            {
                //Kunden har endnu ikke lagt noget i kurven
                //Så vi opretter en ny kurv
                sb = new ShoppingBasket();
            }
            else
            {
                //Vi henter kurven fra sessionen
                sb = JsonSerializer.Deserialize<ShoppingBasket>(basket);
            }
            return sb;

        }
        /// <summary>
        /// Gemmer indkøbskurven på sessionen
        /// </summary>
        /// <param name="basket">Kurven der gemmes</param>
        protected void SaveBasket(ShoppingBasket basket)
        {
            HttpContext.Session.SetString("Basket", JsonSerializer.Serialize(basket));

        }
    }
}
