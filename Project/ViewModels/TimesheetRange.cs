using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.ViewModels
{
    public class TimesheetRange
    {
        public List<Timesheet> items { get; set; }
        public bool Is_Editable { get; set; }
    }
}
