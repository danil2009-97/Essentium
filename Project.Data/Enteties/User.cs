using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.Enteties
{
    public class User: IdentityUser<int>
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }

        public string ManagerEmail { get; set; }

        public bool Is_Contractor { get; set; }

        public int Last_Accept_Year { get; set; } = 0;

        public int Last_Accept_Month { get; set; } = 0;

        public List<Timesheet> Timesheets { get; set; } = new List<Timesheet>();

        public List<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
