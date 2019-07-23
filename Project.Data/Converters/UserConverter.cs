using Project.Data.Dto;
using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Data.Converters
{
    public static class UserConverter
    {
        public static UserDto Convert(User item)
        {
            return new UserDto
            {
                Name = item.Name,
                Surname = item.Surname,
                UserID = item.Id,
                Login = item.UserName,
                Is_Contractor = item.Is_Contractor,
                ManagerEmail = item.ManagerEmail
            };
        }

        public static User Convert(UserDto item)
        {
            return new User
            {
                Name = item.Name,
                Surname = item.Surname,
                UserName = item.Login,
                ManagerEmail = item.ManagerEmail,
                Id = item.UserID,
                Is_Contractor = item.Is_Contractor
            };
        }

        public static List<UserDto> Convert(List<User> items)
        {
            return items.Select(a => Convert(a)).ToList();
        }

        public static List<User> Convert(List<UserDto> items)
        {
            return items.Select(a => Convert(a)).ToList();
        }
    }
}
