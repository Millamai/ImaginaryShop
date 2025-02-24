using ImaginaryShop.Controllers;

namespace ImaginaryShop.Model
{
    public class Product
    {
        public int ProductID { get; set; }          // Unik identifikation for produktet
        public string ProductName { get; set; }     // Navn på produktet
        public string Description { get; set; }      // Beskrivelse af produktet
        public decimal Price { get; set; }           // Pris på produktet
        public int StockQuantity { get; set; }       // Antal på lager
        public string Category { get; set; }          // Kategori for produktet
        public string ImageUrl { get; set; }         // URL til produktbilledet
        public DateTime CreatedAt { get; set; }      // Dato for oprettelse
        public DateTime UpdatedAt { get; set; }      // Dato for seneste opdatering

        public string Currency { get; set; }          // Møntfod


        public Product() { }
        public Product(int productId, string productName, string description, decimal price, int stockQuantity, string category, string imageUrl, string currency)
        {
            ProductID = productId;
            ProductName = productName;
            Description = description;
            Price = price;
            Currency = currency;
            StockQuantity = stockQuantity;
            Category = category;
            ImageUrl = imageUrl;
            CreatedAt = DateTime.Now; // Sætter oprettelsesdato til nu
            UpdatedAt = DateTime.Now; // Sætter opdateringsdato til nu
            Currency = currency;
        }
    }
}
