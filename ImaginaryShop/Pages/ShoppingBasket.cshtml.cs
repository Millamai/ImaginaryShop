using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;

namespace ImaginaryShop.Pages
{
    public class ShoppingBasketModel : ShoppingPageModel
    {
        public IActionResult OnGet()
        {

            ShoppingBasket basket = GetShoppingBasket();


            return new JsonResult(new
            {
                total = basket.GetTotal(),
                itemCount = basket.GetQuantity()
            });

        }

        public void OnPost()
        {


      
        }


        public void OnPostAddToCart(int productId)
        {


        }

        /**En vare f�jes til indk�bskurven p� en hurtig m�de... her er der kun tale om et produkt*/
        public IActionResult OnPostQuickAdd(int productId)
        {
            //Henter brugerens indk�bskurv fra sessionen

            ShoppingBasket basket = GetShoppingBasket();

            ProductRepository r = new ProductRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");
            // TODO: Tjek lagerbeholdning med mere
            Product productToBeAdded = r.GetProductById(productId, Currency);

            if (productToBeAdded != null)
            {
                //Produktet findes i databasen
                basket.AddProduct(productToBeAdded,1);
                

            }

            //S� skal kurven gemmes
            SaveBasket(basket);

            return new JsonResult(new
            {
                total = basket.GetTotal(),
                itemCount = basket.GetQuantity()
            });
        }
    }
}
