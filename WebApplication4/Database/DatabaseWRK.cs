using WebApplication4.Models;

using Npgsql;
namespace WebApplication4.Database;

public class DatabaseWRK
{
    public static User GetUserByEmail(string email)
    
    {
        var connectionString = "Host=localhost;Username=admin;Password=admin;Database=practice";
        var dataSource = NpgsqlDataSource.Create(connectionString);
       
        var command = dataSource.CreateCommand($"SELECT * FROM users WHERE login = \'{email}\'");
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
}