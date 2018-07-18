using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace LibSharedRun.Repos
{
    public class RunnerRepo : IRunnerRepo
    {
        private readonly string _cs;

        public RunnerRepo(string cs)
        {
            _cs = cs;
        }



        public async Task InsertRunner(Runner runner)
        {
            using (SqlConnection connection = new SqlConnection(_cs))
            {
                var query = @"
                            INSERT INTO[dbo].[Runner]
                                    ([Username]
                                    ,[LastName]
                                    ,[FirstName]
                                    ,[Birthday]
                                    ,[Sex]
                                    ,[PhotoUri], Id)
                                        VALUES (@Username, @LastName, @Name, @Birthday, @Sex, @PhotoUri, @Id);";

               await connection.QueryAsync(query, new
               {
                   Name = runner.Name,
                   LastName = runner.LastName,
                   PhotoUri = runner.PhotoUri,
                   Birthday = runner.Birthday.Year + "-" + runner.Birthday.Month + "-" + runner.Birthday.Day,
                   Sex = runner.Sex,
                   Username = runner.Username,
                   Id = runner.Id
               });
            }
        }
    }
}
