using ImaginaryShop.Model;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Net.NetworkInformation;
using static ImaginaryShop.Model.User;
using ImaginaryShop.Model.Repos;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    // Opret en ny bruger
    public User CreateUser(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = @"
                INSERT INTO Users (UserName, Email, PasswordHash, FullName, Role, CreatedAt)
                OUTPUT INSERTED.UserId
                VALUES (@UserName, @Email, @PasswordHash, @FullName, @Role, @CreatedAt)";

            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@CreatedAt",DateTime.Now);

                // Execute and get the inserted UserId
                user.UserId = (int)command.ExecuteScalar();
                return user;
            }
        }
    }

    // Hent en bruger baseret på email
    public User GetUserByEmail(string email)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT * FROM Users WHERE Email = @Email";
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
                    }
                }
            }
        }
        return null;
    }
    public User GetUserByUserName(string uname)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT * FROM Users WHERE UserName = @UserName";
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", uname);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
                    }
                }
            }
        }
        return null;
    }

    // Hent en bruger baseret på ID
    public User GetUserById(int userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT * FROM Users WHERE UserId = @UserId";
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapUser(reader);
                    }
                }
            }
        }
        return null;
    }

    // Opdater en bruger
    public User UpdateUser(User user)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = @"
                UPDATE Users 
                SET UserName = @UserName, Email = @Email, PasswordHash = @PasswordHash, FullName = @FullName, Role = @Role
                WHERE UserId = @UserId";

            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@UserId", user.UserId);

                command.ExecuteNonQuery();
                return user;
            }
        }
    }

    // Slet en bruger
    public void DeleteUser(int userId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "DELETE FROM Users WHERE UserId = @UserId";
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.ExecuteNonQuery();
            }
        }
    }

    // Hent alle brugere
    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        using (var connection = new SqlConnection(_connectionString))
        {
            const string query = "SELECT * FROM Users";
            connection.Open();
            using (var command = new SqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(MapUser(reader));
                    }
                }
            }
        }
        return users;
    }

    // Mapper database-læseren til en User-objekt
    private User MapUser(IDataReader reader)
    {
        return new User
        {
            UserId = Convert.ToInt32(reader["UserId"]),
            UserName = reader["UserName"].ToString(),
            Email = reader["Email"].ToString(),
            PasswordHash = reader["PasswordHash"].ToString(),
            FullName = reader["FullName"].ToString(),
            Role = (UserRole)Enum.ToObject(typeof(UserRole), Convert.ToInt32(reader["Role"])),



            CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
            LastLogin = reader["LastLogin"] as DateTime?
        };
    }
}
