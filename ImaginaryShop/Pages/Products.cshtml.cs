using ImaginaryShop.Model;
using ImaginaryShop.Model.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImaginaryShop.Pages
{
    public class ProductsModel : PageModel
    {
        IEnumerable<Product> products;

        public IEnumerable<Product> Products { get => products; set => products = value; }

        public void OnGet()
        {
            ProductRepository r = new ProductRepository("Server=localhost;Database=ImaginaryShop;Integrated Security=True;;Encrypt=False");
            Products = r.GetAllProducts();
        }
    }
}
