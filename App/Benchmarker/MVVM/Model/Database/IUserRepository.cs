using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model.Database
{
    internal interface IUserRepository
    {
        Task<User> Login(User user);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        void InsertUser(User benchmark);
        void UpdateUser(User benchmark);
        void DeleteUser(int id);
    }
}
