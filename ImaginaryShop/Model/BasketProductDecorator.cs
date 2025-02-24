namespace ImaginaryShop.Model
{
    public class BasketProductDecorator
    {


        public Product Product { get; set; }
        public int Quantity { get; set; }
        public BasketProductDecorator(Product p)
        {
            Product = p;
        }
    }
}
