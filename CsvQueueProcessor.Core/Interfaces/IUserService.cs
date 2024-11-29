using CsvQueueProcessor.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Interfaces
{
    public interface IUserService
    {
        Task<int> AddUserAsync(User user);
        Task<User> GetUserByCredentialsAsync(string username, string password);
    }
}
