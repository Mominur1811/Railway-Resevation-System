using Railway_Ticket_Book.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Railway_Ticket_Book.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        PekkaEntities dbObj = new PekkaEntities();
        public ActionResult Admin(vehicle dt)
        {
            if (dt.RailId == 0)
            {
                return View();
            }
            else
            {
                return View(dt);
            }
        }

        [HttpPost]
        public ActionResult AddTrain(vehicle model)
        {
            if (ModelState.IsValid)
            {
                vehicle obj = new vehicle();
                obj.RNm = model.RNm;
                obj.s_start = model.s_start;
                obj.s_des = model.s_des;
                obj.ar_date = model.ar_date;
                obj.A_seat = model.A_seat;
                obj.N_seat = model.N_seat;
                obj.C_seat = model.C_seat;
                obj.A_F = model.A_F;
                obj.B_F = model.B_F;
                obj.C_F = model.C_F;
                obj.R_tm = model.R_tm;
                TempData["AddTrainAlert"] = "You have Successfully added Train.";
                dbObj.vehicles.Add(obj);
                dbObj.SaveChanges();
               // }
                //else
                //{
                  //  obj.RailId = model.RailId;
                   // dbObj.Entry(obj).State = EntityState.Modified;
                   // dbObj.SaveChanges();
               // }
            }
            ModelState.Clear();
            return View("Admin");
        }

        public ActionResult AdminLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(LoginViewModel lg)
        {
            if (lg.nam == "admin@gmail.com" && lg.pass == "admin")
            {
                ModelState.Clear();
                return RedirectToAction("admin");
            }
            ModelState.Clear();
            return View();
        }

        public ActionResult TrainList()
        {

            var res = dbObj.vehicles.ToList();
            return View(res);
        }

        public ActionResult TList()
        {

            var res = dbObj.vehicles.ToList();
            return View(res);
        }



        public ActionResult Delete(vehicle dd)
        {
            var res = dbObj.vehicles.Where(x => x.RailId == dd.RailId).First();
            if (res != null)
            {
                dbObj.vehicles.Remove(res);
                dbObj.SaveChanges();
            }
            var list = dbObj.vehicles.ToList();
            return View("TrainList", list);
        }

        public ActionResult UserDetails()
        {
            var res = dbObj.customers.ToList();
            return View(res);
        }

        public ActionResult UpdateTrain(vehicle dt)
        {

            return View(dt);
        }

        [HttpPost]
        public ActionResult UpdateTrainI(vehicle model)
        {
            if (ModelState.IsValid)
            {
                vehicle obj = new vehicle();
                obj.RNm = model.RNm;
                obj.s_start = model.s_start;
                obj.s_des = model.s_des;
                obj.ar_date = model.ar_date;
                obj.A_seat = model.A_seat;
                obj.N_seat = model.N_seat;
                obj.C_seat = model.C_seat;
                obj.A_F = model.A_F;
                obj.B_F = model.B_F;
                obj.C_F = model.C_F;
                obj.R_tm = model.R_tm;
                obj.RailId = model.RailId;
                dbObj.Entry(obj).State = EntityState.Modified;
                dbObj.SaveChanges();
                TempData["UpdateTrainAlert"] = "Successfully Updated";
                ModelState.Clear();

            }
            else
            {
                TempData["UpdateTrainFail"] = "Update with valid data.";
            }
            return View("UpdateTrain", model);
        }

    }
}