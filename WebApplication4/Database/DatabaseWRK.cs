    using System.Data;
    using System.IO.Pipes;
    using WebApplication4.Models;

    using Npgsql;

    using System.Text;
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

            await reader0.DisposeAsync();
            await command0.DisposeAsync();

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
                    SpecialtyCode = readSpeciality.GetString(1),
                    IsPhysics = readSpeciality.GetBoolean(7),
                    SpecialtyFacultyAndName = readSpeciality.GetString(2) + ';' + readSpeciality.GetString(3)
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

                await takeSpeciality.DisposeAsync();
                await readSpeciality.DisposeAsync();
                await takeTime.DisposeAsync();
                await readTime.DisposeAsync();
                ans.Add(newUserSpecialtiy);
            }

            await takeUserSpeciality.DisposeAsync();
            await readUserSpeciality.DisposeAsync();
            await dataSource.CloseAsync();
            await dataSource2.CloseAsync();
            await dataSource3.CloseAsync();
            await dataSource4.CloseAsync();
            return ans;
        }
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
                    SpecialtyCode = readSpecialties.GetString(1),
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

            await findSpecialties.DisposeAsync();
            await readUser.DisposeAsync();
            await using var findExam = new NpgsqlCommand("SELECT * FROM exams WHERE user_id = @p1", dataSource1)
            {
                Parameters =
                {
                    new("p1", userId)
                }
            };
            await using var readExam = await findExam.ExecuteReaderAsync();
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

        public static async Task<bool> UpdateUserExamsAsync(Exams exams, string email)
        {
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource2 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            await using var findUser = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
            {
                Parameters =
                {
                    new("p1", email)
                }
            };
            await using var readUser = await findUser.ExecuteReaderAsync();
            int userId = 0;
            if (await readUser.ReadAsync())
            {
                userId = readUser.GetInt32(0);
            }
            else
            {
                await dataSource.CloseAsync();
                await dataSource2.CloseAsync();
                await readUser.DisposeAsync();
                return false;
            }
            
            await readUser.DisposeAsync();
            await using var findExam = new NpgsqlCommand("SELECT * FROM exams WHERE user_id = @p1", dataSource)
            {
                Parameters =
                {
                    new("p1", userId)
                }
            };
            await using var readExam = await findExam.ExecuteReaderAsync();
            if (await readExam.ReadAsync())
            {
                await using var editExam = new NpgsqlCommand(
                    "UPDATE exams SET IsRussian = @p2, IsPhysics = @p3, LanguageExam = @p4, MathExam = @p5, PhysicsExam = @p6, LanguageScore = @p7, MathScore = @p8, PhysicsScore = @p9, LanguageMark = @p10, MathMark = @p11, PhysicsMark = @p12 WHERE user_id = @p1",
                    dataSource2)
                {
                    Parameters =
                    {
                        new("p1", userId),
                        new("p2", exams.IsRussian),
                        new("p3", exams.IsPhysics),
                        new("p4", exams.LanguageExam),  
                        new("p5", exams.MathExam),
                        new("p6", exams.PhysicsExam),
                        new("p7", exams.LanguageScore),
                        new("p8", exams.MathScore),
                        new("p9", exams.PhysicsScore),
                        new("p10", exams.LanguageMark),
                        new("p11", exams.MathMark),
                        new("p12", exams.PhysicsMark)
                    }
                };
                await editExam.ExecuteNonQueryAsync();
                await editExam.DisposeAsync();
                
            }
            else
            {
                await using var addExam = new NpgsqlCommand(
                    "INSERT INTO exams(user_id, IsRussian, IsPhysics, LanguageExam, MathExam, PhysicsExam, LanguageScore, MathScore, PhysicsScore, LanguageMark, MathMark, PhysicsMark) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12)",
                    dataSource2)
                {
                    Parameters =
                    {
                        new NpgsqlParameter("p1", userId),
                        new NpgsqlParameter("p2", exams.IsRussian),
                        new NpgsqlParameter("p3", exams.IsPhysics),
                        new NpgsqlParameter("p4", exams.LanguageExam),
                        new NpgsqlParameter("p5", exams.MathExam),
                        new NpgsqlParameter("p6", exams.PhysicsExam),
                        new NpgsqlParameter("p7", exams.LanguageScore),
                        new NpgsqlParameter("p8", exams.MathScore),
                        new NpgsqlParameter("p9", exams.PhysicsScore),
                        new NpgsqlParameter("p10", exams.LanguageMark),
                        new NpgsqlParameter("p11", exams.MathMark),
                        new NpgsqlParameter("p12", exams.PhysicsMark)
                    }
                };
                await addExam.ExecuteNonQueryAsync();
                await addExam.DisposeAsync();
                
            }

            await dataSource.CloseAsync();  
            await dataSource2.CloseAsync();
            return true;
        }
        public static async Task<bool> UpdateUserSpecialtiesAsync(UserSpecialties specialties, string email){
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource2 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            await using var findUser = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
            {
                Parameters =
                {
                    new("p1", email)
                }
            };
            await using var readUser = await findUser.ExecuteReaderAsync();
            int userId = 0;
            if (await readUser.ReadAsync())
            {
                userId = readUser.GetInt32(0);
            }
            else
            {
                await dataSource.CloseAsync();
                await dataSource2.CloseAsync();
                await readUser.DisposeAsync();
                return false;
            }

            await readUser.DisposeAsync();

            for (int i = 0; i < specialties.SpecialtiesCodes.Count; i++)
            {
                await using var findSpecialities  = new NpgsqlCommand("SELECT * FROM specialities WHERE code = @p1", dataSource)
                {
                    Parameters =
                    {
                        new("p1", specialties.SpecialtiesCodes[i])
                    }
                };
                await using var readSpecialities = await findSpecialities.ExecuteReaderAsync();
                int specialitiesId = 0;
                if (await readSpecialities.ReadAsync())
                {
                    specialitiesId = readSpecialities.GetInt32(0);
                }

                await findSpecialities.DisposeAsync();
                await readSpecialities.DisposeAsync();
                int points = 0;
                
                await using var findExams  = new NpgsqlCommand("SELECT * FROM exams WHERE user_id = @p1", dataSource)
                {
                    Parameters =
                    {
                        new("p1", userId)
                    }
                };
                await using var readExams = await findExams.ExecuteReaderAsync();
                if (await readExams.ReadAsync())
                {
                    points += readExams.GetInt32(6);
                    points += readExams.GetInt32(7);
                    points += readExams.GetInt32(8);
                }

                await findExams.DisposeAsync();
                await readExams.DisposeAsync();
                
                await using var findUserSpecialities  = new NpgsqlCommand("SELECT * FROM user_specialities WHERE user_id = @p1 AND speciality_id = @p2", dataSource)
                {
                    Parameters =
                    {
                        new("p1", userId),
                        new ("p2",specialitiesId)
                    }
                };
                await using var readUserSpecialities = await findUserSpecialities.ExecuteReaderAsync();
                if (await readUserSpecialities.ReadAsync())
                {
                    await using var editUserSpecialities = new NpgsqlCommand(
                        "UPDATE exams SET priority = @p2, user_points = @p3 WHERE user_id = @p1 AND speciality_id = @p2",
                        dataSource2)
                    {
                        Parameters =
                        {
                            new("p1", userId),
                            new("p2", specialitiesId),
                            new("p3", i),
                            new("p4", points),
                        }
                    };
                    await editUserSpecialities.ExecuteNonQueryAsync();
                    await editUserSpecialities.DisposeAsync();
                }
                else
                {
                    await using var insertUserSpecialities = new NpgsqlCommand(
                        "INSERT INTO user_specialities(user_id, priority, speciality_id, user_points) VALUES (@p1, @p2, @p3, @p4)",
                        dataSource2)
                    {
                        Parameters =
                        {
                            new NpgsqlParameter("p1", userId),
                            new NpgsqlParameter("p2", i),
                            new NpgsqlParameter("p3", specialitiesId),
                            new NpgsqlParameter("p4", points)
                        }
                    };
                    await insertUserSpecialities.ExecuteNonQueryAsync();
                    await insertUserSpecialities.DisposeAsync();
                }
                await findUserSpecialities.DisposeAsync();
                await readUserSpecialities.DisposeAsync();
            }

            await dataSource.CloseAsync();
            await dataSource2.CloseAsync();
            return true;
        }
    }
