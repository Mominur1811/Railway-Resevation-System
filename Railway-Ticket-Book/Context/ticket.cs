using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Railway_Ticket_Book.Context
{
    public class ticket
    {
        public int TrnId { get; set; }
        public Nullable<int> Ac_chair { get; set;}
        public Nullable<int> shovon { get; set; }
        public Nullable<int> vid_s { get; set; }
        public Nullable<int> fair1 { get; set; }
        public Nullable<int> fair2 { get; set; }
        public Nullable<int> fair3 { get; set; }
    }
}