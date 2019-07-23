using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Project.Data.Enteties
{
    public class Timesheet
    {
        public int UserID { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Day { get; set; }
        [Required]
        public int Bilable { get; set; } = 0;

        public int? Km { get; set; }

        public bool Holiday { get; set; } = false;

        public bool TimeOff { get; set; } = false;

        public bool SpecialLeave { get; set; } = false;

        public bool SickDay { get; set; } = false;

        public string Comments { get; set; }
    }
}
