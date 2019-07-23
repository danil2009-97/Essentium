using Project.Data.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDto>> GetAllAsync();

        Task<UserDto> GetByIdAsync(int id);

        Task<UserDto> GetByLoginAsync(string login);

        Task<UserDto> CreateAsync(UserDto item);

        Task<bool> UpdateAsync(UserDto item);

        Task<bool> DeleteByIdAsync(int id);

        Task<bool> DeleteByLoginAsync(string login);
    }
}
