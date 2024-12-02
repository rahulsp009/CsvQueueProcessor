using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CsvQueueProcessor.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> AddUserAsync(User user)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "INSERT INTO [User] (Username, Password, Email) VALUES (@Username, @Password, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    return await db.ExecuteScalarAsync<int>(sqlQuery, user);
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
        }

        public async Task<User> GetUserByCredentialsAsync(string username, string password)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT * FROM [User] WHERE Username = @Username AND Password = @Password";
                try
                {
                    return await db.QueryFirstOrDefaultAsync<User>(sqlQuery, new { Username = username, Password = password });
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
        }
    }
}
