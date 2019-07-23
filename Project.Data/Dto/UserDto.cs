using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Dto
{
    public class UserDto
    {
        public int UserID { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string ManagerEmail { get; set; }

        public bool Is_Contractor { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }
    }
}
