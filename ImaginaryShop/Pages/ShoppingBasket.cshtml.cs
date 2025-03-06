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

        /// <summary>
        /// Føjer et produkt til brugerens indkøbskurv med et item.
        /// </summary>
        /// <param name="productId">ID på det produkt, der skal tilføjes til kurven.</param>
        /// <returns>
        /// Returnerer en JSON-respons med den opdaterede totalpris og antal varer i kurven.
        /// Hvis brugeren er administrator, returneres en 403 Forbidden.
        /// </returns>
        public IActionResult OnPostQuickAdd(int productId)
        {
            // Henter brugerens indkøbskurv fra sessionen
            ShoppingBasket basket = GetShoppingBasket();

            // Hvis brugeren er administrator, forbydes adgang til at tilføje produkter
            if (User.IsInRole("Admin"))
            {
                return Forbid(); // Returnerer en HTTP 403 Forbidden
            }

            // Opretter et repository-objekt for at hente produktdata fra databasen
            ProductRepository r = new ProductRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");

            // TODO: Implementer tjek af lagerbeholdning, før produktet tilføjes
            Product productToBeAdded = r.GetProductById(productId, Currency);

            if (productToBeAdded != null)
            {
                // Produktet findes i databasen og tilføjes til indkøbskurven med en mængde på 1
                basket.AddProduct(productToBeAdded, 1);
            }

            // Gemmer den opdaterede indkøbskurv i sessionen
            SaveBasket(basket);

            // Returnerer en JSON-respons med den opdaterede totalpris og antal varer i kurven
            return new JsonResult(new
            {
                total = basket.GetTotal(),
                itemCount = basket.GetQuantity()
            });
        }

    }
}
