using System.Diagnostics;
using System.Text;

namespace ImaginaryShop.Model
{
    public class ShoppingBasket
    {

        // TODO: Implementer når brugeren ændrer currency

        public List<BasketProductDecorator> Products { get; set; } = new();

        public ShoppingBasket()
        {

        }

        public double GetTotal()
        {
            return (double)Products.Sum(x => x.Quantity * x.Product.Price);
        }

        public int GetQuantity()
        {
            return Products.Sum(x => x.Quantity);

        }





        /// <summary>
        /// Tilføjer et product til indkøbskurven. Hvis produktet allerede findes, så tælles antallet bare op. Hvis produktet ikke findes, tilføjes det
        /// </summary>
        /// <param name="product">Det product der skal lægge i kurven</param>
        /// <param name="quantity">Antallet af produkter</param>

        public void AddProduct(Product product, int quantity)
        {

            //Quantity skal altid være et positivt tal
            if (quantity < 1)
                quantity = 1;
            //Nu skal vi finde ud af, om det allerede eksisterer
            if (Products.Any(x => x.Product.ProductID == product.ProductID))
            {
                //Her er produktet allerede lagt i kurven, så antallet skal bare tælles een op

                BasketProductDecorator bpd = Products.Find(x => x.Product.ProductID == product.ProductID);
                if (bpd != null)
                {
                    bpd.Quantity += quantity;

                }

            }
            else
            {
                //Tilføjes til kurven
                Products.Add(new BasketProductDecorator(product, quantity));
            }
        }


        public string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Products)
            {
                sb.Append(item.Product.ProductName);
            }

            return "Qnt: " + GetQuantity() + ", " + "Total:" + GetTotal();

        }
    }
}
