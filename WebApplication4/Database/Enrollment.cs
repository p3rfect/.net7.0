using Org.BouncyCastle.Crypto.Engines;

namespace WebApplication4.Database;
using Npgsql;
public class Enrollment
{
    private static readonly string ConnectionString = "Host=localhost;Username=admin;Password=admin;Database=practice";
    public static async Task<bool> EnrollmentUsers()
    {
        List<(int, int)> users = new List<(int, int)>();
        await using var dataSource = new NpgsqlConnection(ConnectionString);
        dataSource.Open();

        await using var findUser = new NpgsqlCommand($"SELECT * FROM user_specialities", dataSource);
        await using var readUser = await findUser.ExecuteReaderAsync();

        while (await readUser.ReadAsync())
        {
            users.Add((readUser.GetInt32(0), readUser.GetInt32(3)));
        }

        await findUser.DisposeAsync();
        await readUser.DisposeAsync();
        for (int i = 0; i < users.Count; i++)
        {
            int j = i;
            while (j - 1 > 0 && users[j].Item2 > users[j - 1].Item2)
            {
                (int, int) dp = (users[j].Item1, users[j].Item2);
                users[j] = users[j - 1];
                users[j - 1] = dp;
                j--;
            }
        }
        
        await dataSource.CloseAsync();
        return true;
    }    
}