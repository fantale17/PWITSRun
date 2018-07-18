using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Lib.Repos.Interfaces;

namespace Lib.Repos
{
    public class PointRepo : IPointRepo
    {
        private readonly string _connectionString;

        public PointRepo(string cs)
        {
            _connectionString = cs;
        }



        public async Task InsertAsync(Point p)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlFormattedDate = p.Time.ToString("yyyy-MM-dd HH:mm:ss.fff");
                var query = @"INSERT INTO [dbo].[Point]([X],[Y],[Time],[IdActivity],[UriSelfie]) " +
                            "VALUES (@Latitude, @Longitude, DATEADD(HOUR, 2, @Time), @IdActivity, @UriSelfie)";

                await connection.QueryAsync(query, new
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Time = sqlFormattedDate,
                    IdActivity = p.IdActivity,
                    UriSelfie = p.UriSelfie
                });
            }
        }


        public async Task InsertSharedPointAsync(Point p)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sqlFormattedDate = p.Time.ToString("yyyy-MM-dd HH:mm:ss.fff");


                var query = @"declare @idActivity as int;
                                SELECT TOP 1 @idActivity = Id FROM dbo.Activity WHERE UriGara = @uriGara;   
                            INSERT INTO [dbo].[Point]([X],[Y],[Time],[IdActivity], IdRunner) " +
                            "VALUES (@Latitude, @Longitude, DATEADD(HOUR, 2, @Time), @idActivity, @IdRunner)";

                await connection.QueryAsync(query, new
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude,
                    Time = sqlFormattedDate,
                    UriSelfie = p.UriSelfie,
                    IdRunner = p.IdRunner,
                    UriGara = p.UriGara
                });
            }
        }


        public async Task InsertSelfieAsync(UriTime uriTime)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"declare @id as int;
                                SELECT TOP 1 @id = Id
                                        FROM dbo.Point
                                        WHERE IdActivity = @idActivity AND Time < DATEADD(HOUR, 2, @Time)
                                        ORDER BY Time DESC;

                                UPDATE
                                    [dbo].[Point] SET [UriSelfie] = @Uri WHERE ID = @id";

                await connection.QueryAsync(query, new
                {
                    Uri = uriTime.SelfieUri,
                    idActivity = uriTime.IdActivity,
                    Time = uriTime.Time
                });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Point>> GetActivityPointsAsync(int activityId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT [X] as Latitude,[Y] as Longitude,[Time],[IdActivity],[UriSelfie]
                                    FROM [dbo].[Point] where IdActivity = @Id";
                var result = await connection.QueryAsync<Point>(query, new {Id = activityId});
                return result.ToList();
            }
        }
    }
}
