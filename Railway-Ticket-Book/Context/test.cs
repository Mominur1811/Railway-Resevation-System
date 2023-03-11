using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railway_Ticket_Book.Context
{
    public class test
    {
        public string passname { get; set; }
        public string trainname { get; set; }
        public string pStart { get; set; }
        public string pDest { get; set; }
        public int amm { get; set; }

        public string a_ticket { get; set; }
        public string b_ticket { get; set; }
        public string c_ticket { get; set; }
        public Nullable<System.DateTime> pass_date { get; set; }
        public Nullable<System.TimeSpan> pass_time { get; set; }
    }
}