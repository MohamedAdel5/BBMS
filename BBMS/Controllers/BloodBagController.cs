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
    public class BloodBagController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();

        // GET: BloodBag
        public DBManager dbm;
        public BloodBagController()
        {
            dbm = new DBManager();
        }
        public ActionResult Index()
        {
            var blood_bag = db.blood_bag.Include(b => b.hospital).Include(b => b.donor).Include(b => b.shift);
            return View(blood_bag.ToList());
        }

        // GET: BloodBag/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blood_bag blood_bag = db.blood_bag.Find(id);
            if (blood_bag == null)
            {
                return HttpNotFound();
            }
            return View(blood_bag);
        }

        // GET: BloodBag/Create
        public ActionResult Create()
        {
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username");
            ViewBag.national_id = new SelectList(db.donors, "national_id", "name");
            ViewBag.blood_camp_id = new SelectList(db.shifts, "blood_camp_id", "shift_manager_username");
            return View();
        }

        // POST: BloodBag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BloodBag blood_bag,string blood_date)
        {
            string q1=$"INSERT INTO blood_bag(national_id,blood_bag_date,blood_camp_id,hospital_id,notes,blood_type)" + 
                      $"VALUES('{blood_bag.national_id}','{blood_date}','{blood_bag.blood_camp_id}','{blood_bag.hospital_id}','{blood_bag.notes}','{blood_bag.blood_type}')";

                 dbm.ExecuteNonQuery(q1);
 
                return RedirectToAction("Index");
        }

        // GET: BloodBag/Edit/5
       

        // POST: BloodBag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       

        // GET: BloodBag/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blood_bag blood_bag = db.blood_bag.Find(id);
            if (blood_bag == null)
            {
                return HttpNotFound();
            }
            return View(blood_bag);
        }

        // POST: BloodBag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            blood_bag blood_bag = db.blood_bag.Find(id);
            db.blood_bag.Remove(blood_bag);
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
