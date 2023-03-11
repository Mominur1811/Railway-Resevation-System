using Railway_Ticket_Book.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Rotativa.MVC;
using System.IO;

namespace Railway_Ticket_Book.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        PekkaEntities db = new PekkaEntities();
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(searchTrain p)
        {
            var res = db.vehicles.Where(x => x.s_start == p.station1 && x.s_des == p.station2 && x.ar_date==p.journeyDate).ToList();
            if(res.Count==0)
            {
                TempData["TrainNotFound"] = "No train Found to this route";
                return View();
            }
            return View("searT", res);
        }


        public ActionResult seat_selection(vehicle dt)
        {

            vehicle tmp = new vehicle();
            return View();
        }

        public ActionResult trainSelection(ticket tt)
        {

            vehicle tmp = new vehicle();
            tmp.RailId = tt.TrnId;
            tmp.A_seat = 0;
            tmp.N_seat = 0;
            tmp.C_seat = 0;
            
            return View(tmp);
        }

        [HttpPost]
        public ActionResult BuyTicket(vehicle tic)
        {
            var res = db.vehicles.Where(x => x.RailId == tic.RailId).First();
            int cnt = (int)(tic.A_seat + tic.N_seat + tic.C_seat);
            if (cnt<=4 && tic.A_seat<=res.A_seat && tic.N_seat<=res.N_seat && tic.C_seat<=res.C_seat)
            {

                tranHistory th = new tranHistory();
                res.A_seat -= tic.A_seat;
                res.N_seat -= tic.N_seat;
                res.C_seat -= tic.C_seat;
                th.Tran_no = res.RailId;
                th.Ac = tic.A_seat;
                th.Non = tic.N_seat;
                th.cab = tic.C_seat;

                int total1 = tic.A_seat.HasValue ? (int)(tic.A_seat) : 0;
                int total2= tic.N_seat.HasValue ? (int)(tic.N_seat) : 0;
                int total3=tic.C_seat.HasValue ? (int)(tic.C_seat) : 0;

                int total4 = res.A_F.HasValue ? (int)(res.A_F) : 0;
                int total5 = res.B_F.HasValue ? (int)(res.B_F) : 0;
                int total6 = res.C_F.HasValue ? (int)(res.C_F) : 0;

                int total = (total1 * total4) + (total2 * total5) + (total3 * total6);
                

                th.amount = total;
                th.s_from = res.s_start;
                th.Des = res.s_start;

                //db.Entry(res).State = EntityState.Modified;
                //db.SaveChanges();
                return View("Payment",th);

            }
            ticket tt = new ticket();
            tt.TrnId = tic.RailId;
            tt.Ac_chair = tic.A_seat;
            tt.shovon = tic.N_seat;
            tt.vid_s = tic.C_seat;
            TempData["SelectTiketInValid"] = "Reqest is not Valid";
            return View("trainSelection", tic);
        }

        public ActionResult Payment(tranHistory th)
        {
            
            return View(th);
        }

        public ActionResult pdfgener(test tx)
        {
            return View(tx);
        }



        [HttpPost]
        public ActionResult SavePayment(tranHistory th)
        {
            int t = (int)th.Tran_no;
            var res = db.vehicles.Where(x => x.RailId == t).First();

            res.A_seat -= th.Ac;
            res.N_seat -= th.Non;
            res.C_seat -= th.cab;
            db.Entry(res).State = EntityState.Modified;
            db.SaveChanges();


            db.tranHistories.Add(th);
            db.SaveChanges();

            test tx = new test();
            tx.amm = (int)th.amount;

            string x1="0";
            if(th.Ac!=0)
            {
                x1 = (res.A_seat - th.Ac + 1).ToString() + "-" + res.A_seat.ToString();
            }
           
            string x2="0";
            if(th.Non!=0)
            {
                x2 = (res.N_seat - th.Non + 1).ToString() + "-" + res.N_seat.ToString();
            }
            
            string x3="0";
            if (th.cab != 0)
            {
                x3 = (res.C_seat - th.cab + 1).ToString() + "-" + res.C_seat.ToString();
            }
            

            tx.a_ticket = x1;
            tx.b_ticket = x2;
            tx.c_ticket = x3;
            tx.trainname = "Simanto Express";
            tx.passname = User.Identity.Name;
            tx.pStart = res.s_start;
            tx.pDest = res.s_des;
            tx.pass_date = res.ar_date;
            tx.pass_time = res.R_tm;

            var pdf = new ActionAsPdf("pdfgener", tx);

            string s = (string)Session["var4"];
            string s2 = "rahman1807011@stud.kuet.ac.bd";
            string pass = "ku1811ET395!";
            using (MailMessage mm = new MailMessage(s2,s))
            {
                mm.Subject="Ticket Confirmation";
                mm.Body = "Hello " + User.Identity.Name + ",\n" + "You have succesfully purchased ticket. Please, check your history for ticket.\n Download ticket ...";

                MemoryStream memoryStream = new MemoryStream();
                Byte[] pdfData = pdf.BuildPdf(ControllerContext);
                MemoryStream pdfStream = new MemoryStream(pdfData);
                Attachment pdfdat = new Attachment(pdfStream, "ticket.pdf", "application/pdf");
                mm.Attachments.Add(pdfdat);

                mm.IsBodyHtml = false;

                using(SmtpClient smtp=new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential cred = new NetworkCredential(s2, pass);
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = cred;
                    smtp.Send(mm);
                }
            }

            //return View("Index");

            return new ActionAsPdf("pdfgener", tx);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            String s = (string)Session["var4"];
            ViewBag.Message = s;

            return View();
        }
        
        public ActionResult userHistory()
        {
            string s = (string)Session["var4"];
            var res = db.tranHistories.Where(x=>x.gmail==s).ToList();
            return View(res);
        }
    }
}