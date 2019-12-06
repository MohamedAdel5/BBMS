using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BBMS.db_access;
using BBMS.Models;
using BBMS.ViewModels;


namespace BBMS.Controllers
{
    public class hospitalsController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();
        public DBManager dbm;
        public hospitalsController()
        {
            dbm = new DBManager();
        }
        // GET: hospitals
        public ActionResult Index()
        {
            var hospitals = db.hospitals.Include(h => h.login);
            return View(hospitals.ToList());
        }

        // GET: hospitals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital hospital = db.hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }

        // GET: hospitals/Create
        public ActionResult Create()
        {
            ViewBag.username = new SelectList(db.logins, "username", "user_type");
            return View();
        }

        // POST: hospitals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hospital hospital)
        {
            //Dictionary<string, object> Parameters = new Dictionary<string, object>();
            //Parameters.Add("@username", hospital.username);
            //Parameters.Add("@user_pass", hospital.password);
            //Parameters.Add("@hospital_name", hospital.hospital_name);
            //Parameters.Add("@phone", hospital.phone);
            //Parameters.Add("@city", hospital.city);
            //Parameters.Add("@governorate", hospital.governorate);
            //db.insert_hospital(hospital.username, hospital.password, hospital.hospital_name, hospital.phone, hospital.city, hospital.governorate);

            string q1 = $"INSERT INTO login (username,user_pass, user_type) " + $"Values ('{hospital.username}', HASHBYTES('SHA2_512','{hospital.password}'),'H')";
                dbm.ExecuteNonQuery(q1);
            string q2 = "INSERT INTO Hospital (username,hospital_name, phone, City, governorate)"+$"Values('{hospital.username}','{hospital.hospital_name}','{hospital.phone}','{hospital.city}','{hospital.governorate}')";
            dbm.ExecuteNonQuery(q2);


            if (ModelState.IsValid)
            {

                return RedirectToAction("Index");
            }

            ViewBag.username = new SelectList(db.logins, "username", "user_type", hospital.username);
            return View(hospital);
        }

        // GET: hospitals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital hospital = db.hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = new SelectList(db.logins, "username", "user_type", hospital.username);
            return View(hospital);
        }

        // POST: hospitals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(hospital hospital)
        {
            string q1 = $"UPDATE hospital SET hospital_name = '{hospital.hospital_name}',phone = '{hospital.phone}',City= '{hospital.city}',governorate='{hospital.governorate}' WHERE username='{hospital.username}'";
            dbm.ExecuteNonQuery(q1);

            if (ModelState.IsValid)
            {


           //     db.Entry(hospital).State = EntityState.Modified;
             //   db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.username = new SelectList(db.logins, "username", "user_type", hospital.username);
            return View(hospital);
        }

        // GET: hospitals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital hospital = db.hospitals.Find(id);
            if (hospital == null)
            {
                return HttpNotFound();
            }
            return View(hospital);
        }

        // POST: hospitals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hospital hospital = db.hospitals.Find(id);
            db.hospitals.Remove(hospital);
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
