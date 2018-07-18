using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LibSharedRun.Repos
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
                            "([Name],[IdOrganizer] ,[CreationDate],[Place],[Type] ,[UriGara])  " +
                            "VALUES " +
                            "( @Name," +
                            "@IdOrganizer, DATEADD(HOUR, 2, GETDATE())," +
                            "@Place,@Type,@UriGara)";
                await connection.QueryAsync<Activity>(query, act);
            }
        }




        public async Task<IEnumerable<Activity>> GetAllRacesAsync()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    "Select [UriGara], [Name], [Place] from dbo.Activity where Type = 2";

                return (await connection.QueryAsync<Activity>(query)).ToList();
            }
        }




        public async Task<Activity> GetRaceDetailsAsync(string uriGara)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query =
                    "Select Id, [Name], [Place], CreationDate, UriGara from dbo.Activity where UriGara = @uriGara";

                return await connection.QueryFirstOrDefaultAsync<Activity>(query, new{ uriGara = uriGara});
            }
        }



        public async Task RegisterRacePartecipant(string uriGara, int runnerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
               

                var query = @"declare @idActivity as int;
                                SELECT TOP 1 @idActivity = Id FROM dbo.Activity WHERE UriGara = @uriGara;
                            INSERT INTO[dbo].[RunnerActivity] (IdRunner, IdActivity)
                                VALUES
                                ( @IdRunner,
                                @idActivity)";
                await connection.QueryAsync(query, new{uriGara = uriGara, IdRunner = runnerId});
            }
        }
    }
}
