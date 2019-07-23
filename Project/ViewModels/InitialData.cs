using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class InitialData
    {
        public string ManagerEmail { get; set; }
        public List<Timesheet> Timesheets { get; set; }
        public bool Is_Editable { get; set; }
        public List<Notification> Notifications { get; set; }
    }
}
