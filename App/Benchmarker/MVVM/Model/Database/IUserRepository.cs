using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model.Database
{
    internal interface IUserRepository
    {
        Task<string> Login(User user);
        void Logout(string token);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<User> GetUserByToken(string token);
        void InsertUser(User benchmark);
        void UpdateUser(User benchmark);
        void DeleteUser(int id);
    }
}
