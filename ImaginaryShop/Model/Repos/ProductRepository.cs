using Microsoft.Data.SqlClient;
using System.Data;
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
        public Product GetProductById(int productId, string currency)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
             

                SqlCommand command = new SqlCommand("GetProductInCurrency", connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                command.Parameters.AddWithValue("@Currency", currency);
                command.CommandType = CommandType.StoredProcedure;
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Product
                    {
                        ProductID = reader.GetInt32(reader.GetOrdinal("ProductId")),
                        ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                        StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                        Category = reader.GetString(reader.GetOrdinal("Category")),
                        ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                        Currency = reader.GetString(reader.GetOrdinal("NickName"))
                    };
                }
                return null;
            }
        }

        //public IEnumerable<Product> GetAllProducts(string currency)
        //{
        //    var products = new List<Product>();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string query = "SELECT * FROM Products";
        //        SqlCommand command = new SqlCommand(query, connection);

        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {

        //            products.Add(new Product
        //            {
        //                ProductID = (int)reader["ProductID"],
        //                ProductName = reader["ProductName"].ToString(),
        //                Description = reader["Description"].ToString(),
        //                Price = (decimal)reader["Price"],
        //                StockQuantity = (int)reader["StockQuantity"],
        //                Category = reader["Category"].ToString(),
        //                ImageUrl = reader["ImageUrl"].ToString(),
        //                CreatedAt = (DateTime)reader["CreatedAt"],
        //                UpdatedAt = (DateTime)reader["UpdatedAt"]
        //            });
        //        }
        //    }
        //    return products;
        //}

        public IEnumerable<Product> GetAllProducts(string currency)
        {
            var products = new List<Product>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Debug.WriteLine("CUR" +currency);
                using (SqlCommand cmd = new SqlCommand("GetAllProductInCurrency", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Tilføj parametre til stored procedure
                    cmd.Parameters.AddWithValue("@Currency", currency);



                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                                Currency = reader.GetString(reader.GetOrdinal("NickName"))
                            });
                        }
                    }



                }
                return products;
            }
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
