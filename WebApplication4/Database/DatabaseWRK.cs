    using System.Data;
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

        public static async Task<List<Specialty>> GetUserSpecialtiesAsync(string email)
        {
            var ans = new List<Specialty>();
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource2 = new NpgsqlConnection(ConnectionString);
            await using var dataSource3 = new NpgsqlConnection(ConnectionString);
            await using var dataSource4 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            await dataSource3.OpenAsync();
            await dataSource4.OpenAsync();
            
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
                return ans;
            }

            await using var takeUserSpeciality =
                new NpgsqlCommand("SELECT * FROM user_specialities WHERE user_id = @p1", dataSource2)
                {
                    Parameters =
                    {
                        new("p1", userId)
                    }
                };
            await using var readUserSpeciality = await takeUserSpeciality.ExecuteReaderAsync();

            while (await readUserSpeciality.ReadAsync())
            {
                int specialityId = readUserSpeciality.GetInt32(2);
                await using var takeSpeciality =
                    new NpgsqlCommand("SELECT * FROM speciality WHERE speciality_id  = @p1", dataSource3)
                    {
                        Parameters =
                        {
                            new("p1", specialityId)
                        }
                    };
                await using var readSpeciality = await takeSpeciality.ExecuteReaderAsync();
                Specialty newUserSpecialtiy = new Specialty()
                {
                    IsPhysics = readSpeciality.GetBoolean(7),
                    SpecialtyFacultyAndName = readSpeciality.GetString(2) + ' ' + readSpeciality.GetString(3)
                };
                await using var takeTime =
                    new NpgsqlCommand("SELECT * FROM specialityPeriodAndTime WHERE speciality_id  = @p1", dataSource4)
                    {
                        Parameters =
                        {
                            new("p1", specialityId)
                        }
                    };
                await using var readTime = await takeTime.ExecuteReaderAsync();
                while (await readTime.ReadAsync())
                {
                    newUserSpecialtiy.FinancingFormPeriod.Add(readTime.GetString(1));
                }

                ans.Add(newUserSpecialtiy);
            }

            return ans;
        }

        /* public static async Task<bool> UpdateUserInfoAsync(UserInfo user, string email)
        {
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await using var takeUser = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource);
            takeUser.Parameters.AddWithValue("@p1", email);
            await using var readUser = await takeUser.ExecuteReaderAsync();
            if (await readUser.ReadAsync())
            {
                int userId = readUser.GetInt32(0);
                await using var findInfo = new NpgsqlCommand("SELECT * FROM UserInfo WHERE user_id = @p1", dataSource);
                findInfo.Parameters.AddWithValue("@p1", email);
                await using var readInfo = await findInfo.ExecuteReaderAsync();
                if (await readInfo.ReadAsync())
                {
                    await using var command = 
                        new NpgsqlCommand("UPDATE UserInfo SET speciality_name = @newName WHERE speciality_id = @id", dataSource);
                }
            }

            await dataSource.DisposeAsync();
            return false;
        }*/
        public static async Task<List<Specialty>> GetAllSpecialtiesAsync()
        {
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource2 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            var ans = new List<Specialty>();
            await using var findSpecialties = new NpgsqlCommand("SELECT * FROM speciality", dataSource);
            await using var readSpecialties = await findSpecialties.ExecuteReaderAsync();
            while (await readSpecialties.ReadAsync())
            {
                Specialty newSpecialties = new Specialty()
                {
                    IsPhysics = readSpecialties.GetBoolean(7),
                    SpecialtyFacultyAndName = readSpecialties.GetString(3) + ' ' + readSpecialties.GetString(2)
                };
                int dp = readSpecialties.GetInt32(0);
                await using var takeTime =
                    new NpgsqlCommand("SELECT * FROM specialityPeriodAndTime WHERE speciality_id  = @p1", dataSource2)
                    {
                        Parameters =
                        {
                            new("p1", dp)
                        }
                    };
                await using var readTime = await takeTime.ExecuteReaderAsync();
                while (await readTime.ReadAsync())
                {
                    newSpecialties.FinancingFormPeriod.Add(readTime.GetString(1));
                }
                ans.Add(newSpecialties);
                await takeTime.DisposeAsync();
                await readTime.DisposeAsync();

            }

            await readSpecialties.DisposeAsync();
            await findSpecialties.DisposeAsync();
            await dataSource.CloseAsync();
            await dataSource2.CloseAsync();
            return ans;
        }

        public static async Task<Exams> GetUserExamsAsync(string email)
        {
            Exams ans = new Exams();
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource1 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource1.OpenAsync();
            await using var findSpecialties = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
            {
                Parameters =
                {
                    new("p1", email)
                }
            };
            await using var readUser = await findSpecialties.ExecuteReaderAsync();
            int userId = -1;
            if (await readUser.ReadAsync())
            {
                userId = readUser.GetInt32(0);
            }

            await readUser.DisposeAsync();
            await using var findExam = new NpgsqlCommand("SELECT * FROM user_specialities WHERE user_id = @p1", dataSource1)
            {
                Parameters =
                {
                    new("p1", userId)
                }
            };
            await using var readExam = await findSpecialties.ExecuteReaderAsync();
            if (await readExam.ReadAsync())
            {
                ans.IsRussian = readExam.GetBoolean(1);
                ans.IsPhysics = readExam.GetBoolean(2);
                ans.LanguageExam = readExam.GetString(3);
                ans.MathExam = readExam.GetString(4);
                ans.PhysicsExam = readExam.GetString(5);
                ans.LanguageScore = readExam.GetInt32(6);
                ans.MathScore = readExam.GetInt32(7);
                ans.PhysicsScore = readExam.GetInt32(8);
                ans.LanguageMark = readExam.GetInt32(9);
                ans.MathMark = readExam.GetInt32(10);
                ans.PhysicsMark = readExam.GetInt32(11);
            }

            await readExam.DisposeAsync();
            await dataSource.CloseAsync();
            await dataSource1.CloseAsync();
            return ans;
        }
    }
