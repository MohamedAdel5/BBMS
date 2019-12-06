using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BBMS.Models;
using BBMS.db_access;

namespace BBMS.Controllers
{
    public class BloodCampsController : Controller
    {
        public DBManager dbm;
        public BloodCampsController()
        {
            dbm = new DBManager();
        }
        private EBBMSEntities db = new EBBMSEntities();

        // GET: BloodCamps
        public ActionResult Index()
        {
            //string q1= $"select* from blood_camp;";
            var blood_camp = db.blood_camp.Include(b => b.hospital);
            return View(blood_camp.ToList());
        }

        // GET: BloodCamps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blood_camp blood_camp = db.blood_camp.Find(id);
            if (blood_camp == null)
            {
                return HttpNotFound();
            }
            return View(blood_camp);
        }

        // GET: BloodCamps/Create
        public ActionResult Create()
        {
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username");
            return View();
        }

        // POST: BloodCamps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( BloodCamp blood_camp)
        {

            string q1 = $"insert into blood_camp values('{blood_camp.hospital_id}', '{blood_camp.driver_name}')";
            dbm.ExecuteNonQuery(q1);

            if (ModelState.IsValid)
            {
               // db.blood_camp.Add(blood_camp);
               //db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", blood_camp.hospital_id);
            return View(blood_camp);
        }

        // GET: BloodCamps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blood_camp blood_camp = db.blood_camp.Find(id);
            if (blood_camp == null)
            {
                return HttpNotFound();
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", blood_camp.hospital_id);
            return View(blood_camp);
        }

        // POST: BloodCamps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BloodCamp blood_camp)
        {
            string q1 = $"update blood_camp set driver_name='{blood_camp.driver_name}'where blood_camp_id ={blood_camp.blood_camp_id}";
            dbm.ExecuteNonQuery(q1);
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", blood_camp.hospital_id);
            return View(blood_camp);
        }

        // GET: BloodCamps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            blood_camp blood_camp = db.blood_camp.Find(id);
            if (blood_camp == null)
            {
                return HttpNotFound();
            }
            return View(blood_camp);
        }

        // POST: BloodCamps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            blood_camp blood_camp = db.blood_camp.Find(id);
            db.blood_camp.Remove(blood_camp);
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
