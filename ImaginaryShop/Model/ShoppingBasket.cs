using System.Text;

namespace ImaginaryShop.Model
{
    public class ShoppingBasket
    {

        public List<BasketProductDecorator> Products { get; private set; }
        public ShoppingBasket()
        {
            Products = new List<BasketProductDecorator>();
        }

        public double GetTotal()
        {

            return (double)Products.Sum(x => x.Quantity * x.Product.Price);
        }

        public int GetQuantity()
        {
            return Products.Sum(x => x.Quantity);


        }


        public string ToString() {
            StringBuilder sb = new StringBuilder();

            foreach (var item in Products)
            {
                sb.Append(item.Product.ProductName);
            }



            return "Qnt: " + GetQuantity() + ", " + "Total:" + GetTotal();
        
        }
    }
}
