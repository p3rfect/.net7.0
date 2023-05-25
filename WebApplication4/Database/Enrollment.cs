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
            while (j - 1 >= 0 && users[j].Item2 > users[j - 1].Item2)
            {
                (int, int) dp = (users[j].Item1, users[j].Item2);
                users[j] = users[j - 1];
                users[j - 1] = dp;
                j--;
            }
        }

        List<string> sp = new List<string>();
        for (int i = 0; i < users.Count; i++)
        {
            await using var findPriority = new NpgsqlCommand($"SELECT * FROM user_specialities WHERE user_id = (@p1) AND priority  = @p2", dataSource)
            {
                Parameters =
                {
                    new("p1", users[i].Item1),
                    new ("p2", 1)
                }
            };
            await using var readPriority = await findPriority.ExecuteReaderAsync();
            int k = 0;
            if (await readPriority.ReadAsync())
            {
                if (k == 0)
                {
                    sp.Add(readPriority.GetString(4));
                    k = 1;
                }
            }

            await findPriority.DisposeAsync();
            await readPriority.DisposeAsync();
        }

        for (int i = 0; i < users.Count; i++)
        {
            await using var addStudent = new NpgsqlCommand(
                "INSERT INTO students(user_id, code) VALUES ((@p1), (@p2))",
                dataSource)
            {
                Parameters =
                {
                    new("p1", users[i].Item1),
                    new("p2", sp[i])
                }
            };
            await addStudent.ExecuteNonQueryAsync();
            await addStudent.DisposeAsync();
        }
        
        await dataSource.CloseAsync();
        return true;
    }    
}
