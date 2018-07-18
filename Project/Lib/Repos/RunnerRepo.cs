using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lib.Repos.Interfaces;

namespace Lib.Repos
{
    public class RunnerRepo : IRunnerRepo
    {

        private readonly string _connectionString;

        public RunnerRepo(string cs)
        {
            _connectionString = cs;
        }


        public async Task<int> InsertAsync(Runner r)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO [dbo].[Runner]([Name],[LastName],[PhotoUri], [Birthday], [Sex] ,[Username],[IdAspNet]) " +
                            "VALUES(@Name, @LastName, @PhotoUri, @Birthday, @Sex, @Username, @IdAspNet); SELECT SCOPE_IDENTITY();";

                return await connection.QueryFirstOrDefaultAsync<int>(query, new
                {
                    Name = r.Name,
                    LastName = r.LastName,
                    PhotoUri = r.PhotoUri,
                    Birthday = r.Birthday.Year + "-" + r.Birthday.Month + "-" + r.Birthday.Day,
                    Sex = r.Sex,
                    Username = r.Username,
                    IdAspNet = r.IdAspNet
                });
            }
        }



        public async Task<Runner> GetUserAsync(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT [IdAspNet] ,[Email],[IdRunner] as Id,[Username],[Name], [LastName], [Sex], [Birthday], [PhotoUri] " +
                            "FROM[dbo].[UserView] where IdAspNet = @Id";

                return await connection.QueryFirstAsync<Runner>(query, new { Id = id });
            }
        }


        public async Task SetNameAsync(int runnerId, string name)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE[dbo].[Runner] SET[Name] = @Name WHERE Id = @id";

                await connection.QueryAsync(query, new { id = runnerId, Name = name});
            }
        }


        public async Task SetLastNameAsync(int runnerId, string lastName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE[dbo].[Runner] SET[LastName] = @lastName WHERE Id = @id";

                await connection.QueryAsync(query, new { id = runnerId, lastName = lastName });
            }
        }


        public async Task SetSexAsync(int runnerId, int? sex)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE[dbo].[Runner] SET[Sex] = @sex WHERE Id = @id";

                await connection.QueryAsync(query, new { id = runnerId, sex = sex });
            }
        }


        public async Task SetBirthdayAsync(int runnerId, DateTime birthday)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE[dbo].[Runner] SET[Birthday] = @birthday WHERE Id = @id";

                await connection.QueryAsync(query, new { id = runnerId, birthday = birthday });
            }
        }


        public async Task SetPhotoUriAsync(int runnerId, string photoUri)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE[dbo].[Runner] SET[PhotoUri] = @photoUri WHERE Id = @id";

                await connection.QueryAsync(query, new { id = runnerId, photoUri = photoUri });
            }
        }

        public async Task GetPhotoUriAsync(string aspNetId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT R.PhotoUri from [dbo].[Runner] as R join dbo.AspNetUsers as A on A.Id = R.IdAspnet WHERE R.IdAspNet = @id";

                await connection.QueryAsync(query, new { id = aspNetId});
            }
        }
    }
}
