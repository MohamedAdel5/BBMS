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
    public class HospitalProvidesServiceController : Controller
    {
        public DBManager dbm;
        public HospitalProvidesServiceController()
        {
            dbm = new DBManager();
        }

        private EBBMSEntities db = new EBBMSEntities();

        // GET: HospitalProvidesService
        public ActionResult Index()
        {
            var hospital_provides_service = db.hospital_provides_service.Include(h => h.hospital).Include(h => h.service);

            return View(hospital_provides_service.ToList());
        }

        // GET: HospitalProvidesService/Details/5
        public ActionResult Details(int? id,string service_name)
        {
            string q1 = $"SELECT*FROM hospital_provides_service where hospital_id='{id}' and service_name ='{service_name}'";
            var h1 =dbm.ExecuteReader(q1);
       
            return View(h1);
        }

        // GET: HospitalProvidesService/Create
        public ActionResult Create()
        {
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username");
            ViewBag.service_name = new SelectList(db.services, "name", "name");
            return View();
        }

        // POST: HospitalProvidesService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Hospital_Provide_Service   HPS )
        {
            string q1 = $"INSERT INTO hospital_provides_service(hospital_id,service_name,value)" +
                        $"VALUES('{HPS.hospital_id}','{HPS.service_name}','{HPS.value}')";

            dbm.ExecuteNonQuery(q1);

            return RedirectToAction("Index");
            

           
        }

        // GET: HospitalProvidesService/Edit/5
        public ActionResult Edit(int? id, string service_name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(id,service_name);
            if (hospital_provides_service == null)
            {
                return HttpNotFound();
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", hospital_provides_service.hospital_id);
            ViewBag.service_name = new SelectList(db.services, "name", "name", hospital_provides_service.service_name);
            return View(hospital_provides_service);
        }

        // POST: HospitalProvidesService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hospital_id,service_name,value")] hospital_provides_service hospital_provides_service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospital_provides_service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", hospital_provides_service.hospital_id);
            ViewBag.service_name = new SelectList(db.services, "name", "name", hospital_provides_service.service_name);
            return View(hospital_provides_service);
        }

        // GET: HospitalProvidesService/Delete/5
        public ActionResult Delete(int? id,string service_name)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(id,service_name);
            if (hospital_provides_service == null)
            {
                return HttpNotFound();
            }
            return View(hospital_provides_service);
        }

        // POST: HospitalProvidesService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(id);
            db.hospital_provides_service.Remove(hospital_provides_service);
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
