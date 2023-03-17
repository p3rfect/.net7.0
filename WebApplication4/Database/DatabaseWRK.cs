using WebApplication4.Models;

using Npgsql;

namespace WebApplication4.Database;

public class DatabaseWRK
{
    public static User GetUserByEmail(string email)
    {
        var connectionString = "Host=localhost;Username=admin;Password=admin;Database=practice";  
        var dataSource = new NpgsqlConnection(connectionString);
        dataSource.Open();
        var command = new NpgsqlCommand($"SELECT * FROM users WHERE login = (@p1)", dataSource)
        {
            Parameters =
            {
                new("p1", email)
            }
        };
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            /*
             return new User()
            {
                Id = reader.GetInt32(0),
                
                Email = email,
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
             */
            var res = new User()
            {
                Id = reader.GetInt32(0),
                
                Email = email,
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
            reader.Dispose();
            dataSource.Dispose();
            return res;
        }
        else
        { 
            reader.Dispose();
            dataSource.Dispose();
            return new User() { Id = null, Email = null, Password = null, Role = null };
        }
    }
    
    //returns false if such user already exists
    public static async Task<bool> AddNewUserAsync(User user)
    {
        var connectionString = "Host=localhost;Username=admin;Password=admin;Database=practice";
        await using var dataSource = new NpgsqlConnection(connectionString);
        dataSource.Open();
        await using var command = new NpgsqlCommand($"SELECT * FROM users WHERE login = (@p1)", dataSource)
        {
            Parameters =
            {
                new("p1", user.Email)
            }
        };
        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return false;
        }
        // else
        // {
        await reader.DisposeAsync();
        await using var addСommand = new NpgsqlCommand("INSERT INTO users(login, password, role) VALUES ((@p1), (@p2), (@p3))",
            dataSource)
        {
            Parameters =
            {
                new("p1", user.Email),
                new("p2", user.Password),
                new("p3", user.Role)
            }
        };
        await addСommand.ExecuteNonQueryAsync();
        return true;
        // }
    }
}