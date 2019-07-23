using Project.Data.Dto;
using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abc.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<object> Login(string login, string password);

        Task<object> Register(UserDto item);
    }

}
