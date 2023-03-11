using Railway_Ticket_Book.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Railway_Ticket_Book.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        PekkaEntities entity = new PekkaEntities();
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel credentials)
        {
            bool userExist = entity.customers.Any(x => x.Main == credentials.nam && x.Pword == credentials.pass);
            customer u = entity.customers.FirstOrDefault(x => x.Main == credentials.nam && x.Pword == credentials.pass);

            if (userExist)
            {
                FormsAuthentication.SetAuthCookie(u.FName, false);
                Session["var4"] = u.Main;
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email or Password is wrong");

            return View();
        }
        [HttpPost]
        public ActionResult Signup(customer userI)
        {
            entity.customers.Add(userI);
            entity.SaveChanges();
            return RedirectToAction("Login");

        }
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult forgetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult forgetPassword(passwr nw)
        {
            string s = nw.fgmail;
            string s2 = "provide your mail";
            string pass = "mail password";

            bool userExist = entity.customers.Any(x => x.Main == s);

            if(userExist)
            {
                var res = entity.customers.Where(x => x.Main == s).First();
                using (MailMessage mm = new MailMessage(s2, s))
                {
                    mm.Subject = "Reset Password";

                    mm.Body = "Hello ,\n" + "You have requested for reset your password\n"+"Your Old Password: "+res.Pword;


                    mm.IsBodyHtml = false;

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential cred = new NetworkCredential(s2, pass);
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = cred;
                        smtp.Send(mm);
                    }
                    TempData["GmailFound"] = "Password has been sent to your email.";
                }
            }
            else
            {
                TempData["GmailNotFound"] = "No account exist to this email.";
            }


            return View();
        }
    }
}