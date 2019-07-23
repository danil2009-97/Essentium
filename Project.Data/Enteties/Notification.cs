using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Enteties
{
    public class Notification
    {
        public int ID { get; set; }

        public int UserID { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public bool Is_Approved { get; set; }

        public bool Is_Seen { get; set; }
    }
}
