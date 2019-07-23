using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Data.Converters;
using Project.Data.Dto;
using Project.Data.Enteties;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly UserManager<User> _userManager;

        public UserRepository(UserManager<User> um)
        {
            _userManager = um;
        }

        public async Task<UserDto> CreateAsync(UserDto item)
        {
            User user = UserConverter.Convert(item);
            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return null;
            return UserConverter.Convert(
                await _userManager.FindByNameAsync(item.Login));
        }

        public async Task<bool> DeleteByLoginAsync(string login)
        {
            User user = await _userManager.FindByNameAsync(login);
            if (user == null)
                return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return false;
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return UserConverter.Convert
                (await _userManager.Users.ToListAsync());
        }

        public async Task<UserDto> GetByLoginAsync(string login)
        {
            return UserConverter.Convert(
                await _userManager.FindByNameAsync(login));
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            return UserConverter.Convert(
                await _userManager.FindByIdAsync(id.ToString()));
        }

        public async Task<bool> UpdateAsync(UserDto item)
        {
            User user = UserConverter.Convert(item);
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
