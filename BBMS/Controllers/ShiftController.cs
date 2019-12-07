using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BBMS;
using BBMS.db_access;
using BBMS.Models;

namespace BBMS.Controllers
{
    public class ShiftController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();
        public DBManager dbm;
        public ShiftController()
        {
            dbm = new DBManager();
        }

        // GET: Shift
        public ActionResult Index()
        {
            var shifts = db.shifts.Include(s => s.blood_camp).Include(s => s.shift_manager);
            return View(shifts.ToList());
        }

        // GET: Shift/Details/5
        public ActionResult Details(int? id,DateTime? date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

           shift shift = db.shifts.Find(id,date);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // GET: Shift/Create
        public ActionResult Create()
        {
            ViewBag.blood_camp_id = new SelectList(db.blood_camp, "blood_camp_id", "driver_name");
            ViewBag.shift_manager_username = new SelectList(db.shift_manager, "username", "name");
            return View();
        }

        // POST: Shift/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shift shift,string start,string end, string shift_date)
        {
            string q1=$"INSERT INTO shift(blood_camp_id ,shift_date,shift_manager_username,start_hour,finish_hour,city,governorate)" +
           $"VALUES ('{shift.blood_camp_id}','{shift_date}','{shift.shift_manager_username}','{start}','{end}','{shift.city}','{shift.governorate}')";
            dbm.ExecuteNonQuery(q1);

            //if (ModelState.IsValid)
            //{
            //  //  db.shifts.Add(shift);
            //  // db.SaveChanges();
               
            //}
            return RedirectToAction("Index");
           // ViewBag.blood_camp_id = new SelectList(db.blood_camp, "blood_camp_id", "driver_name", shift.blood_camp_id);
           // ViewBag.shift_manager_username = new SelectList(db.shift_manager, "username", "name", shift.shift_manager_username);
           // return View(shift);
        }

        // GET: Shift/Edit/5
        public ActionResult Edit(int? id, DateTime? date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shift shift = db.shifts.Find(id,date);
            if (shift == null)
            {
                return HttpNotFound();
            }
            ViewBag.blood_camp_id = new SelectList(db.blood_camp, "blood_camp_id", "driver_name", shift.blood_camp_id);
            ViewBag.shift_manager_username = new SelectList(db.shift_manager, "username", "name", shift.shift_manager_username);
            return View(shift);
        }

        // POST: Shift/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "blood_camp_id,shift_date,shift_manager_username,start_hour,finish_hour,city,governorate")] shift shift)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.blood_camp_id = new SelectList(db.blood_camp, "blood_camp_id", "driver_name", shift.blood_camp_id);
            ViewBag.shift_manager_username = new SelectList(db.shift_manager, "username", "name", shift.shift_manager_username);
            return View(shift);
        }

        // GET: Shift/Delete/5
        public ActionResult Delete(int? id, DateTime? date)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shift shift = db.shifts.Find(id,date);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Shift/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id, DateTime? date)
        {
            shift shift = db.shifts.Find(id,date);
            db.shifts.Remove(shift);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
