using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BBMS;

namespace BBMS.Controllers
{
    public class HPSController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();

        // GET: HPS
        public ActionResult Index()
        {
            var hospital_provides_service = db.hospital_provides_service.Include(h => h.hospital).Include(h => h.service);
            return View(hospital_provides_service.ToList());
        }

        // GET: HPS/Details/5
        public ActionResult Details(int? id,int?h_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(h_id,id);
            if (hospital_provides_service == null)
            {
                return HttpNotFound();
            }
            return View(hospital_provides_service);
        }

        // GET: HPS/Create
        public ActionResult Create()
        {
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username");
            ViewBag.service_id_p = new SelectList(db.services, "service_id", "name");
            return View();
        }

        // POST: HPS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "hospital_id,service_id_p,value,service_name")] hospital_provides_service hospital_provides_service)
        {
            if (ModelState.IsValid)
            {
                db.hospital_provides_service.Add(hospital_provides_service);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", hospital_provides_service.hospital_id);
            ViewBag.service_id_p = new SelectList(db.services, "service_id", "name", hospital_provides_service.service_id_p);
            return View(hospital_provides_service);
        }

        // GET: HPS/Edit/5
        public ActionResult Edit(int? id,int? h_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(h_id,id);
            if (hospital_provides_service == null)
            {
                return HttpNotFound();
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", hospital_provides_service.hospital_id);
            ViewBag.service_id_p = new SelectList(db.services, "service_id", "name", hospital_provides_service.service_id_p);
            return View(hospital_provides_service);
        }

        // POST: HPS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "hospital_id,service_id_p,value,service_name")] hospital_provides_service hospital_provides_service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hospital_provides_service).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hospital_id = new SelectList(db.hospitals, "hospital_id", "username", hospital_provides_service.hospital_id);
            ViewBag.service_id_p = new SelectList(db.services, "service_id", "name", hospital_provides_service.service_id_p);
            return View(hospital_provides_service);
        }

        // GET: HPS/Delete/5
        public ActionResult Delete(int? id,int?h_id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(h_id,id);
            if (hospital_provides_service == null)
            {
                return HttpNotFound();
            }
            return View(hospital_provides_service);
        }

        // POST: HPS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,int h_id)
        {
            hospital_provides_service hospital_provides_service = db.hospital_provides_service.Find(h_id,id);
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
