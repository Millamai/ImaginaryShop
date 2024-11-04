using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace ImaginaryShop.Model.Repos
{
    public class ProductRepository
    {
        private string connectionString;

        public ProductRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        // Create
        public void AddProduct(Product product)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Products (ProductName, Description, Price, StockQuantity, Category, ImageUrl, CreatedAt, UpdatedAt) VALUES (@ProductName, @Description, @Price, @StockQuantity, @Category, @ImageUrl, @CreatedAt, @UpdatedAt)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", product.ProductName);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                command.Parameters.AddWithValue("@Category", product.Category);
                command.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Read
        public Product GetProductById(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products WHERE ProductID = @ProductID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", productId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        StockQuantity = (int)reader["StockQuantity"],
                        Category = reader["Category"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString(),
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        UpdatedAt = (DateTime)reader["UpdatedAt"]
                    };
                }
                return null;
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Products";
                SqlCommand command = new SqlCommand(query, connection);
              
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    products.Add(new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        StockQuantity = (int)reader["StockQuantity"],
                        Category = reader["Category"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString(),
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        UpdatedAt = (DateTime)reader["UpdatedAt"]
                    });
                }
            }
            return products;
        }

        // Update
        public void UpdateProduct(Product updatedProduct)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Products SET ProductName = @ProductName, Description = @Description, Price = @Price, StockQuantity = @StockQuantity, Category = @Category, ImageUrl = @ImageUrl, UpdatedAt = @UpdatedAt WHERE ProductID = @ProductID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductName", updatedProduct.ProductName);
                command.Parameters.AddWithValue("@Description", updatedProduct.Description);
                command.Parameters.AddWithValue("@Price", updatedProduct.Price);
                command.Parameters.AddWithValue("@StockQuantity", updatedProduct.StockQuantity);
                command.Parameters.AddWithValue("@Category", updatedProduct.Category);
                command.Parameters.AddWithValue("@ImageUrl", updatedProduct.ImageUrl);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                command.Parameters.AddWithValue("@ProductID", updatedProduct.ProductID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Delete
        public void DeleteProduct(int productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Products WHERE ProductID = @ProductID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ProductID", productId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
