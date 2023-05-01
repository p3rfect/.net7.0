using System.IO.Pipes;
using WebApplication4.Models;

using Npgsql;

namespace WebApplication4.Database;

public class DatabaseWRK
{
    private static readonly string ConnectionString = "Host=localhost;Username=admin;Password=admin;Database=practice";

    public static async Task<User> GetUserByEmailAsync(string email)
    {
        await using var dataSource = new NpgsqlConnection(ConnectionString);
        dataSource.Open();

        await using var command = new NpgsqlCommand($"SELECT * FROM users WHERE login = (@p1)", dataSource)
        {
            Parameters =
            {
                new("p1", email)
            }
        };
        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            var res = new User()
            {
                Id = reader.GetInt32(0),

                Email = email,
                Password = reader.GetString(2),
                Role = reader.GetString(3)
            };
            await reader.DisposeAsync();
            await dataSource.DisposeAsync();
            return res;
        }
        else
        {
            await reader.DisposeAsync();
            await dataSource.DisposeAsync();
            return new User() { Id = null, Email = null, Password = null, Role = null };
        }
    }

    //returns false if such user already exists
    public static async Task<bool> AddNewUserAsync(User user)
    {
        await using var dataSource = new NpgsqlConnection(ConnectionString);
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

        await reader.DisposeAsync();
        await using var addСommand = new NpgsqlCommand(
            "INSERT INTO users(login, password, role) VALUES ((@p1), (@p2), (@p3))",
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
    }
    
    public static async Task<List<Specialties>> GetUserSpecialtiesAsync(string email)
    {
        var Ans = new List<Specialties>();
        await using var dataSource = new NpgsqlConnection(ConnectionString);
        await dataSource.OpenAsync();
        
        await using var command0 = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource);
        command0.Parameters.AddWithValue("@p1", email);
        await using var reader0 = await command0.ExecuteReaderAsync();
        
        int userId = 0;
        if (await reader0.ReadAsync())
        {
            userId = reader0.GetInt32(0);
        }
        else
        {
            return Ans;
        }

        await using var takeUserSpeciality =
            new NpgsqlCommand("SELECT * FROM user_specialities WHERE user_id = @p1", dataSource);
        takeUserSpeciality.Parameters.AddWithValue("@p1", userId);
        await using var readUserSpeciality = await takeUserSpeciality.ExecuteReaderAsync();
        
        while (await readUserSpeciality.ReadAsync())
        {
            int specialityId = readUserSpeciality.GetInt32(2);
            await using var takeSpeciality =
                new NpgsqlCommand("SELECT * FROM speciality WHERE speciality_id  = @p1", dataSource);
            takeSpeciality.Parameters.AddWithValue("@p1", specialityId);
            await using var readSpeciality = await takeSpeciality.ExecuteReaderAsync();
            Specialties newUserSpecialtiy = new Specialties()
            {
                IsPhysics = readSpeciality.GetBoolean(7),
                SpecialtyFacultyAndName = readSpeciality.GetString(2) + ' ' + readSpeciality.GetString(3)
            };
            await using var takeTime =
                new NpgsqlCommand("SELECT * FROM specialityPeriodAndTime WHERE speciality_id  = @p1", dataSource);
            takeTime.Parameters.AddWithValue("@p1", specialityId);
            await using var readTime = await takeTime.ExecuteReaderAsync();
            while (await readTime.ReadAsync())
            {
                newUserSpecialtiy.FinancingFormPeriod.Add(readTime.GetString(1));
            }
            Ans.Add(newUserSpecialtiy);
        }
        return Ans;
    }
}
