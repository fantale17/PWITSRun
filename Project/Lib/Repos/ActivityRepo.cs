using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lib.Repos.Interfaces;

namespace Lib.Repos
{
    public class ActivityRepo : IActivityRepo
    {

        private readonly string _connectionString;


        public ActivityRepo(string connectionString)
        {
            _connectionString = connectionString;
        }


        
        public async Task InsertAsync(Activity act)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
               // string sqlFormattedDate = act.CreationDate.ToString("yyyy-MM-dd HH:mm:ss.fff");

                var query = "INSERT INTO [dbo].[Activity]" +
                            "([Name],[IdRunner] ,[CreationDate],[Place],[Type] ,[UriGara], [Status])  " +
                            "VALUES " +
                            "( @Name," +
                            "@IdRunner, DATEADD(HOUR, 2, GETDATE())," +
                            "@Place,@Type,@UriGara, 0)";
                await connection.QueryAsync<Activity>(query, act);
            }
        }






        public async Task<IEnumerable<Activity>> GetTrainingsAsync(int idRunner)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    "Select [Id], [Name], [Place], [Type], [CreationDate], [Status], UriGara from dbo.Activity where IdRunner = @id";

                return (await connection.QueryAsync<Activity>(query, new{ id = idRunner})).ToList();
            }
        }



        public async Task OpenTrainingAsync(int idActivity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE [dbo].[Activity] SET [Status] = 1 WHERE Id = @id";

                await connection.QueryAsync(query, new { id = idActivity});
            }
        }



        public async Task CloseTrainingAsync(int idActivity)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE [dbo].[Activity] SET [Status] = 2 WHERE Id = @id";

                await connection.QueryAsync(query, new{ id = idActivity});
            }
        }



    }
}
