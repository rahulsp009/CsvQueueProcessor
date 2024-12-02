using CsvQueueProcessor.Core.Entities;
using CsvQueueProcessor.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvQueueProcessor.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<int> AddUserAsync(User user)
        {
            try
            {
                return await _userRepository.AddUserAsync(user);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<User> GetUserByCredentialsAsync(string username, string password)
        {
            try
            {
                return await _userRepository.GetUserByCredentialsAsync(username, password);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
