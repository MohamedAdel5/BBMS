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
    public class DHIController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();

        // GET: DHI
        public DBManager dbm;
        public DHIController()
        {
            dbm = new DBManager();
        }

        public ActionResult Index()
        {
            var donor_health_info = db.donor_health_info.Include(d => d.donor).Include(d => d.hospital);
            return View(donor_health_info.ToList());
        }

        // GET: DHI/Details/5
        public ActionResult Details(long? id, int? id2, DateTime? id3)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            donor_health_info donor_health_info = db.donor_health_info.Find(id,id2,id3);
            if (donor_health_info == null)
            {
                return HttpNotFound();
            }
            return View(donor_health_info);
        }

        // GET: DHI/Create
        public ActionResult Create()
        {
            ViewBag.national_id = new SelectList(db.donors, "national_id", "name");
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username");
            return View();
        }

        // POST: DHI/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DHI DHI,string report_date)
        {
           
            string q1 = $"INSERT INTO donor_health_info(national_id, hospital_id, report_date, blood_pressure, glucose_level, notes)" +
              $"VALUES('{DHI.national_id}','{DHI.hospital_id}','{report_date}','{DHI.blood_pressure}','{DHI.glucose_level}','{DHI.notes}')";

            dbm.ExecuteNonQuery(q1);
            
            return RedirectToAction("Index");

        }

        // GET: DHI/Edit/5
        public ActionResult Edit(long? id,int? id2,DateTime? id3 )
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            donor_health_info donor_health_info = db.donor_health_info.Find(id,id2,id3);
            if (donor_health_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.national_id = new SelectList(db.donors, "national_id", "name", donor_health_info.national_id);
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", donor_health_info.hospital_id);
            return View(donor_health_info);
        }

        // POST: DHI/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "national_id,hospital_id,report_date,blood_pressure,glucose_level,notes")] donor_health_info donor_health_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donor_health_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.national_id = new SelectList(db.donors, "national_id", "name", donor_health_info.national_id);
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", donor_health_info.hospital_id);
            return View(donor_health_info);
        }

        // GET: DHI/Delete/5
        public ActionResult Delete(long? id, int? id2, DateTime? id3)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            donor_health_info donor_health_info = db.donor_health_info.Find(id,id2,id3);
            if (donor_health_info == null)
            {
                return HttpNotFound();
            }
            return View(donor_health_info);
        }

        // POST: DHI/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long? id, int? id2, DateTime? id3)
        {
            donor_health_info donor_health_info = db.donor_health_info.Find(id,id2,id3);
            db.donor_health_info.Remove(donor_health_info);
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
