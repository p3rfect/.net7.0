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
                    SpecialtyFacultyAndName = readSpecialties.GetString(3) + ';' + readSpecialties.GetString(2)
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
            await using var dataSource3 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            await dataSource3.OpenAsync();
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
                await dataSource3.CloseAsync();
                await readUser.DisposeAsync();
                return false;
            }

            await readUser.DisposeAsync();
            await findUser.DisposeAsync();

            for (int i = 0; i < specialties.SpecialtiesCodes.Count; i++)
            {
                await using var findSpecialities  = new NpgsqlCommand("SELECT * FROM specialities WHERE code = @p1", dataSource2)
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
                
                await using var findExams  = new NpgsqlCommand("SELECT * FROM exams WHERE user_id = @p1", dataSource2)
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
                
                await using var findUserSpecialities  = new NpgsqlCommand("SELECT * FROM user_specialities WHERE user_id = @p1 AND speciality_id = @p2", dataSource2)
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
                        dataSource3)
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
                        dataSource3)
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
            await dataSource3.CloseAsync();
            return true;
        }

         public static async Task<bool> UpdateUserInfoAsync(UserInfo user, string email)
        {
            await using var dataSource = new NpgsqlConnection(ConnectionString);
            await using var dataSource2 = new NpgsqlConnection(ConnectionString);
            await using var dataSource3 = new NpgsqlConnection(ConnectionString);
            await dataSource.OpenAsync();
            await dataSource2.OpenAsync();
            await dataSource3.OpenAsync();
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
                await dataSource3.CloseAsync();
                await readUser.DisposeAsync();
                return false;
            }

            await readUser.DisposeAsync();
            await findUser.DisposeAsync();

            await using var findInfo = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
            {
                Parameters =
                {
                    new("p1", email)
                }
            };
            await using var readInfo = await findInfo.ExecuteReaderAsync();

            if (await readInfo.ReadAsync())
            {
//UPDATE userInfo SET Lastname = @p2, Lastnamelat = @p3, Firstname = @p4, Firstnamelat = @p5, surname = @p6, Birthday = @p7, IsMale = @p8, IsSingle = @p9, DocumentType = @p10, IdentyNumber = @p11, Series = @p12, Number = @p13, DateOfIssue = @p14, Validity = @p15, IssuedBy = @p16, Education = @p17, InstitutionType = @p18, Document = @p19, Institution = @p20, DocumentNumber = @p21, GraduationDate = @p22, Language = @p23, AverageScore = @p24, PostalCode = @p25, Country = @p26, Region = @p27, District = @p28, LocalityType = @p29, LocalityName = @p30, StreetType = @p31, Street = @p32, HouseNumber = @p33, HousingNumber = @p34, FlatNumber = @p35, PhoneNumber = @p36, Benefits = @p37, FatherType = @p38, FatherLastname = @p39, FatherFirstname = @p40, FatherSurname = @p41, FatherAddress = @p42, MotherType = @p43, MotherLastname = @p44, MotherFirstname = @p45, MotherSurname = @p46, MotherAddress = @p47 WHERE user_id = @p1
                await using var insertUserInfo = new NpgsqlCommand(
                    "UPDATE userInfo SET Lastname = @p2, Lastnamelat = @p3, Firstname = @p4, Firstnamelat = @p5, surname = @p6, Birthday = @p7, IsMale = @p8, IsSingle = @p9, DocumentType = @p10, IdentyNumber = @p11, Series = @p12, Number = @p13, DateOfIssue = @p14, Validity = @p15, IssuedBy = @p16, Education = @p17, InstitutionType = @p18, Document = @p19, Institution = @p20, DocumentNumber = @p21, GraduationDate = @p22, Language = @p23, AverageScore = @p24, PostalCode = @p25, Country = @p26, Region = @p27, District = @p28, LocalityType = @p29, LocalityName = @p30, StreetType = @p31, Street = @p32, HouseNumber = @p33, HousingNumber = @p34, FlatNumber = @p35, PhoneNumber = @p36, Benefits = @p37, FatherType = @p38, FatherLastname = @p39, FatherFirstname = @p40, FatherSurname = @p41, FatherAddress = @p42, MotherType = @p43, MotherLastname = @p44, MotherFirstname = @p45, MotherSurname = @p46, MotherAddress = @p47 WHERE user_id = @p1",
                    dataSource3)
                {
                    Parameters =
                    {
                        new ("p1", userId),
                        new ("p2", user.Lastname),
                        new ("p3", user.LastnameLat),
                        new ("p4", user.Firstname),
                        new ("p5", user.Firstnamelat),
                        new ("p6", user.Surname),
                        new ("p7", user.Birthday),
                        new ("p8", user.IsMale),
                        new ("p9", user.IsSingle),
                        new ("p10", user.DocumentType),
                        new ("p11", user.IdentyNumber),
                        new ("p12", user.Series),
                        new ("p13", user.Number),
                        new ("p14", user.DateOfIssue),
                        new ("p15", user.Validity),
                        new ("p16", user.IssuedBy),
                        new ("p17", user.Education),
                        new ("p18", user.InstitutionType),
                        new ("p19", user.Document),
                        new ("p20", user.Institution),
                        new ("p21", user.DocumentNumber),
                        new ("p22", user.GraduationDate),
                        new ("p23", user.Language),
                        new ("p24", user.AverageScore),
                        new ("p25", user.PostalCode),
                        new ("p26", user.Country),
                        new ("p27", user.Region),
                        new ("p28", user.District),
                        new ("p29", user.LocalityType),
                        new ("p30", user.LocalityName),
                        new ("p31", user.StreetType),
                        new ("p32", user.Street),
                        new ("p33", user.HouseNumber),
                        new ("p34", user.HousingNumber),
                        new ("p35", user.FlatNumber),
                        new ("p36", user.PhoneNumber),
                        new ("p37", user.Benefits),
                        new ("p38", user.FatherType),
                        new ("p39", user.FatherLastname),
                        new ("p40", user.FatherFirstname),
                        new ("p41", user.FatherSurname),
                        new ("p42", user.FatherAddress),
                        new ("p43", user.MotherType),
                        new ("p44", user.MotherLastname),
                        new ("p45", user.MotherFirstname),
                        new ("p46", user.MotherSurname),
                        new ("p47", user.MotherAddress),
                    }
                };
                await insertUserInfo.ExecuteNonQueryAsync();
                await insertUserInfo.DisposeAsync();
            }
            else
            {
                await using var insertUserInfo = new NpgsqlCommand(
                    "INSERT INTO userInfo(user_id, Lastname, Lastnamelat, Firstname, Firstnamelat, surname, Birthday, IsMale, IsSingle, DocumentType, IdentyNumber, Series, Number, DateOfIssue, Validity, IssuedBy, Education, InstitutionType, Document, Institution, DocumentNumber, GraduationDate, Language, AverageScore, PostalCode, Country, Region, District, LocalityType, LocalityName, StreetType, Street, HouseNumber, HousingNumber, FlatNumber, PhoneNumber, Benefits, FatherType, FatherLastname, FatherFirstname, FatherSurname, FatherAddress, MotherType, MotherLastname, MotherFirstname, MotherSurname, MotherAddress) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17, @p18, @p19, @p20, @p21, @p22, @p23, @p24, @p25, @p26, @p27, @p28, @p29, @p30, @p31, @p32, @p33, @p34, @p35, @p36, @p37, @p38, @p39, @p40, @p41, @p42, @p43, @p44, @p45, @p46, @p47", dataSource3)
                {
                    Parameters =
                    {
                        new NpgsqlParameter("p1", userId),
                        new NpgsqlParameter("p2", user.Lastname),
                        new NpgsqlParameter("p3", user.LastnameLat),
                        new NpgsqlParameter("p4", user.Firstname),
                        new NpgsqlParameter("p5", user.Firstnamelat),
                        new NpgsqlParameter("p6", user.Surname),
                        new NpgsqlParameter("p7", user.Birthday),
                        new NpgsqlParameter("p8", user.IsMale),
                        new NpgsqlParameter("p9", user.IsSingle),
                        new NpgsqlParameter("p10", user.DocumentType),
                        new NpgsqlParameter("p11", user.IdentyNumber),
                        new NpgsqlParameter("p12", user.Series),
                        new NpgsqlParameter("p13", user.Number),
                        new NpgsqlParameter("p14", user.DateOfIssue),
                        new NpgsqlParameter("p15", user.Validity),
                        new NpgsqlParameter("p16", user.IssuedBy),
                        new NpgsqlParameter("p17", user.Education),
                        new NpgsqlParameter("p18", user.InstitutionType),
                        new NpgsqlParameter("p19", user.Document),
                        new NpgsqlParameter("p20", user.Institution),
                        new NpgsqlParameter("p21", user.DocumentNumber),
                        new NpgsqlParameter("p22", user.GraduationDate),
                        new NpgsqlParameter("p23", user.Language),
                        new NpgsqlParameter("p24", user.AverageScore),
                        new NpgsqlParameter("p25", user.PostalCode),
                        new NpgsqlParameter("p26", user.Country),
                        new NpgsqlParameter("p27", user.Region),
                        new NpgsqlParameter("p28", user.District),
                        new NpgsqlParameter("p29", user.LocalityType),
                        new NpgsqlParameter("p30", user.LocalityName),
                        new NpgsqlParameter("p31", user.StreetType),
                        new NpgsqlParameter("p32", user.Street),
                        new NpgsqlParameter("p33", user.HouseNumber),
                        new NpgsqlParameter("p34", user.HousingNumber),
                        new NpgsqlParameter("p35", user.FlatNumber),
                        new NpgsqlParameter("p36", user.PhoneNumber),
                        new NpgsqlParameter("p37", user.Benefits),
                        new NpgsqlParameter("p38", user.FatherType),
                        new NpgsqlParameter("p39", user.FatherLastname),
                        new NpgsqlParameter("p40", user.FatherFirstname),
                        new NpgsqlParameter("p41", user.FatherSurname),
                        new NpgsqlParameter("p42", user.FatherAddress),
                        new NpgsqlParameter("p43", user.MotherType),
                        new NpgsqlParameter("p44", user.MotherLastname),
                        new NpgsqlParameter("p45", user.MotherFirstname),
                        new NpgsqlParameter("p46", user.MotherSurname),
                        new NpgsqlParameter("p47", user.MotherAddress),
                    }
                };
                await insertUserInfo.ExecuteNonQueryAsync();
                await insertUserInfo.DisposeAsync();
            }

            await dataSource.CloseAsync();
            await dataSource2.CloseAsync();
            await dataSource3.CloseAsync();
            return true;
        }

         public static async Task<UserInfo> GetUserInfoAsync(string email)
         {
             await using var dataSource = new NpgsqlConnection(ConnectionString);
             await dataSource.OpenAsync();
             await using var findUser = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
             {
                 Parameters =
                 {
                     new("p1", email)
                 }
             };
             await using var readUser = await findUser.ExecuteReaderAsync();
             int userId = -1;
             if (await readUser.ReadAsync())
             {
                 userId = readUser.GetInt32(0);
             }

             UserInfo ans = new UserInfo();
             await findUser.DisposeAsync();
             await readUser.DisposeAsync();
             await using var findUserInfo = new NpgsqlCommand("SELECT * FROM userInfo WHERE user_id = @p1", dataSource)
             {
                 Parameters =
                 {
                     new("p1", email)
                 }
             };
             await using var readUserInfo = await findUserInfo.ExecuteReaderAsync();
             
             if (await readUserInfo.ReadAsync())
             {
                 ans.Lastname = readUserInfo.GetString(1);
                ans.LastnameLat = readUserInfo.GetString(2);
                ans.Firstname = readUserInfo.GetString(3);
                ans.Firstnamelat = readUserInfo.GetString(4);
                ans.Surname = readUserInfo.GetString(5);
                ans.Birthday = readUserInfo.GetString(6);
                ans.IsMale = readUserInfo.GetBoolean(7);
                ans.IsSingle = readUserInfo.GetBoolean(8);
                ans.DocumentType = readUserInfo.GetString(9);
                ans.IdentyNumber = readUserInfo.GetString(10);
                ans.Series = readUserInfo.GetString(11);
                ans.Number = readUserInfo.GetString(12);
                ans.DateOfIssue = readUserInfo.GetString(13);
                ans.Validity = readUserInfo.GetString(14);
                ans.IssuedBy = readUserInfo.GetString(15);
                ans.Education = readUserInfo.GetString(16);
                ans.InstitutionType = readUserInfo.GetString(17);
                ans.Document = readUserInfo.GetString(18);
                ans.Institution = readUserInfo.GetString(19);
                ans.DocumentNumber = readUserInfo.GetString(20);
                ans.GraduationDate = readUserInfo.GetString(21);
                ans.Language = readUserInfo.GetString(22);
                ans.AverageScore = readUserInfo.GetInt32(23);
                ans.PostalCode = readUserInfo.GetString(24);
                ans.Country = readUserInfo.GetString(25);
                ans.Region = readUserInfo.GetString(26);
                ans.District = readUserInfo.GetString(27);
                ans.LocalityType = readUserInfo.GetString(28);
                ans.LocalityName = readUserInfo.GetString(29);
                ans.StreetType = readUserInfo.GetString(30);
                ans.Street = readUserInfo.GetString(31);
                ans.HouseNumber = readUserInfo.GetString(32);
                ans.HousingNumber = readUserInfo.GetString(33);
                ans.FlatNumber = readUserInfo.GetString(34);
                ans.PhoneNumber = readUserInfo.GetString(35);
                ans.Benefits = readUserInfo.GetString(36);
                ans.FatherType = readUserInfo.GetString(37);
                ans.FatherLastname = readUserInfo.GetString(38);
                ans.FatherFirstname = readUserInfo.GetString(39);
                ans.FatherSurname = readUserInfo.GetString(40);
                ans.FatherAddress = readUserInfo.GetString(41);
                ans.MotherType = readUserInfo.GetString(42);
                ans.MotherLastname = readUserInfo.GetString(43);
                ans.MotherFirstname = readUserInfo.GetString(44);
                ans.MotherSurname = readUserInfo.GetString(45);
                ans.MotherAddress = readUserInfo.GetString(46);
             }

             await readUserInfo.DisposeAsync();
             await findUserInfo.DisposeAsync();
             await dataSource.CloseAsync();
             return ans;
         }

         public static async Task<List<string>> GetAllUsersEmailsAsync()
         {
             await using var dataSource = new NpgsqlConnection(ConnectionString);
             await dataSource.OpenAsync();
             await using var findUser = new NpgsqlCommand("SELECT * FROM users", dataSource);
             List<string> ans = new List<string>();
             await using var readUser = await findUser.ExecuteReaderAsync();
             while (await readUser.ReadAsync())
             {
                 ans.Add(readUser.GetString(1));
             }
             await dataSource.CloseAsync();
             return ans;
         }
         // public static async Task<(UserInfo, Exams, UserSpecialties)> GetUser(string email)
         // {
         //     return (await GetUserInfoAsync(email),await GetUserExamsAsync(email), await GetUserSpecialtiesAsync(email));
         // }
         public static async Task<bool> ConfirmUserAsync(string email){
             await using var dataSource = new NpgsqlConnection(ConnectionString);
             await dataSource.OpenAsync();
             await using var editUser = new NpgsqlCommand(
                 "UPDATE users SET Confirm = @p2 WHERE login = @p1", dataSource)
             {
                 Parameters =
                 {
                     new("p1", email),
                     new("p2", true),
                 }
             };
             await editUser.ExecuteNonQueryAsync();
             await editUser.DisposeAsync();
             await dataSource.CloseAsync(); 
             return true;
         }
         public static async Task<bool> AcceptUserAsync(string email){
             await using var dataSource = new NpgsqlConnection(ConnectionString);
             await dataSource.OpenAsync();
             await using var editUser = new NpgsqlCommand(
                 "UPDATE users SET Accept = @p2 WHERE login = @p1", dataSource)
             {
                 Parameters =
                 {
                     new("p1", email),
                     new("p2", true),
                 }
             };
             await editUser.ExecuteNonQueryAsync();
             await editUser.DisposeAsync();
             await dataSource.CloseAsync(); 
             return true;
         }
         public static async Task<bool> DeleteUserAsync(string email){
             await using var dataSource = new NpgsqlConnection(ConnectionString);
             await dataSource.OpenAsync();
             
             await using var findUser = new NpgsqlCommand("SELECT * FROM users WHERE login = @p1", dataSource)
             {
                 Parameters =
                 {
                     new("p1", email)
                 }
             };
             await using var readUser = await findUser.ExecuteReaderAsync();
             int userId = -1;
             if (await readUser.ReadAsync())
             {
                 userId = readUser.GetInt32(0);
             }
             
             using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
             {
                 await connection.OpenAsync();

                 string deleteQuery = "DELETE FROM users WHERE id = @userId";
                 using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                 {
                     command.Parameters.AddWithValue("@userId", userId);

                     int rowsAffected = 0;
                     do
                     {
                         rowsAffected = await command.ExecuteNonQueryAsync();
                     }
                     while (rowsAffected > 0);
                 }

                 connection.Close();
             }
             using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
             {
                 await connection.OpenAsync();

                 string deleteQuery = "DELETE FROM user_specialities WHERE user_id = @userId";
                 using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                 {
                     command.Parameters.AddWithValue("@userId", userId);

                     int rowsAffected = 0;
                     do
                     {
                         rowsAffected = await command.ExecuteNonQueryAsync();
                     }
                     while (rowsAffected > 0);
                 }

                 connection.Close();
             }
             using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
             {
                 await connection.OpenAsync();

                 string deleteQuery = "DELETE FROM exams WHERE user_id = @userId";
                 using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                 {
                     command.Parameters.AddWithValue("@userId", userId);

                     int rowsAffected = 0;
                     do
                     {
                         rowsAffected = await command.ExecuteNonQueryAsync();
                     }
                     while (rowsAffected > 0);
                 }

                 connection.Close();
             }
             using (NpgsqlConnection connection = new NpgsqlConnection(ConnectionString))
             {
                 await connection.OpenAsync();

                 string deleteQuery = "DELETE FROM userInfo WHERE user_id = @userId";
                 using (NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection))
                 {
                     command.Parameters.AddWithValue("@userId", userId);

                     int rowsAffected = 0;
                     do
                     {
                         rowsAffected = await command.ExecuteNonQueryAsync();
                     }
                     while (rowsAffected > 0);
                 }

                 connection.Close();
             }
             
             await dataSource.CloseAsync(); 
             return true;
         }
    }
/*
IsMale BOOLEAN,
IsSingle BOOLEAN,
DocumentType VARCHAR(255),
IdentyNumber VARCHAR(255),
Series VARCHAR(255),
Number VARCHAR(255),
DateOfIssue VARCHAR(255),
Validity VARCHAR(255),
IssuedBy VARCHAR(255),
Education VARCHAR(255),
InstitutionType VARCHAR(255),
Document VARCHAR(255),
Institution VARCHAR(255),
DocumentNumber VARCHAR(255),
GraduationDate VARCHAR(255),
Language VARCHAR(255),
AverageScore INTEGER,
PostalCode
Country
Region
District
LocalityType
LocalityName
StreetType
Street
HouseNumber
HousingNumber
FlatNumber
PhoneNumber
Benefits
FatherType
FatherLastname
FatherFirstname
FatherSurname
FatherAddress
MotherType
MotherLastname
MotherFirstname
MotherSurname
MotherAddress
*/
