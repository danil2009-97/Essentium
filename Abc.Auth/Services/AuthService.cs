using Abc.Auth.Interfaces;
using Microsoft.AspNetCore.Identity;
using Project.Data.Converters;
using Project.Data.Dto;
using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Abc.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IJwtGenerator _jwt;

        public AuthService(SignInManager<User> sim, UserManager<User> um, IJwtGenerator jwt)
        {
            _signInManager = sim;
            _userManager = um;
            _jwt = jwt;
        }

        public async Task<object> Login(string login, string password)
        {
            if (login == null || password == null)
                return null;

            var result = await _signInManager.PasswordSignInAsync(login, password, false, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByNameAsync(login);
                return await _jwt.GenerateJwt(appUser);
            }
            return null;
        }

        public async Task<object> Register(UserDto item)
        {
            var user = UserConverter.Convert(item);
            var result = await _userManager.CreateAsync(user, item.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await _jwt.GenerateJwt(user);
            }

            return null;
        }
    }



}
