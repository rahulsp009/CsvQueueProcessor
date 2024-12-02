using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CsvQueueProcessor.Infrastructure.Repositories
{
    public class FileProcessingRepository : IFileProcessingRepository
    {
        private readonly string _connectionString;

        public FileProcessingRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<FileProcessing>> GetAllFileProcessingStatuses()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "SELECT Id, FileName, StatusCode, ProcessedDate FROM FileProcessing ORDER BY ProcessedDate DESC";
                try
                {
                    return await db.QueryAsync<FileProcessing>(sqlQuery);
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
        }

        public async Task<int> InsertFileProcessingStatus(FileProcessing fileProcessing)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sqlQuery = "INSERT INTO FileProcessing (FileName, StatusCode, ProcessedDate) VALUES (@FileName, @StatusCode, @ProcessedDate); SELECT CAST(SCOPE_IDENTITY() as int)";
                try
                {
                    return await db.ExecuteScalarAsync<int>(sqlQuery, fileProcessing);
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
        }
    }
}
