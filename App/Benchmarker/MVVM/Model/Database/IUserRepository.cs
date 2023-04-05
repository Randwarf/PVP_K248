using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarker.MVVM.Model.Database
{
    internal interface IUserRepository
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        void InsertUser(User benchmark);
        void UpdateUser(User benchmark);
        void DeleteUser(int id);
    }
}
