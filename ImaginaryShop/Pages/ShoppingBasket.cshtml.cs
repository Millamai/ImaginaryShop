using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Text.Json;

namespace ImaginaryShop.Pages
{
    public class ShoppingBasketModel : PageModel
    {
        public void OnGet()
        {
        }

        public void OnPost()
        {


            Debug.WriteLine("ID ");
        }


        public void OnPostAddToCart(int productId)
        {


            Debug.WriteLine("ID *** " + productId);
        }


        public IActionResult OnPostAdd(int productId)
        {

            //Henter brugerens inkdøbskurv fra sessionen
            var basket = HttpContext.Session.GetString("Basket");
            ShoppingBasket sb;
            if (basket == null)
            {
                Debug.WriteLine("Basket er null");

                //Kunden har endnu ikke lagt noget i kurven
                //Så vi opretter en ny kurv
                sb = new ShoppingBasket();
            }
            else
            {
                Debug.WriteLine("Basket er ikke null");

                //Vi henter kurven fra sessionen
                sb = JsonSerializer.Deserialize<ShoppingBasket>(basket);
            }

            //Tilføjer produktet til kurven
            //Først skal vi finde ud af, om produktet allerede ligger i kurven
            ProductRepository r = new ProductRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");

            if (sb.Products.Any(x => x.Product.ProductID == productId))
            {
                //Her er der allerede et eksisterende produkt
          
                sb.Products.Find(x => x.Product.ProductID == productId).Quantity +=1;

                Debug.WriteLine("1 op");

            }
            else
            {

        //        sb.Products.Add(new BasketProductDecorator(r.GetProductById(productId)));
                Debug.WriteLine("Ny");

            }


            //Læg kurven tilbage på sessionen
            HttpContext.Session.SetString("Basket", JsonSerializer.Serialize(sb));
            Debug.WriteLine("Set");
            Debug.WriteLine(sb.ToString());

            return new JsonResult(new
            {
                total = sb.GetTotal(),
                itemCount = sb.GetQuantity()
            }) ;
        }
    }
}
