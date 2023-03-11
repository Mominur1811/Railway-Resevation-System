using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railway_Ticket_Book.Context
{
    public class searchTrain
    {
        public string station1 { get; set; }
        public string station2 { get; set; }
        public Nullable<System.DateTime> journeyDate { get; set; }
    }
}