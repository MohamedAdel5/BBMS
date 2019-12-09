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
    
    public class ServiceController : Controller
    {
       // convert C = new convert();
        private EBBMSEntities db = new EBBMSEntities();
        
        public DBManager dbm;
        public ServiceController()
        {
            dbm = new DBManager();
        }
        // GET: Service
        public ActionResult Index()
        {
            //DataTable dt = dbm.ExecuteReader("Select*from service");
            //List<Service> ServiceList = new List<Service>();
            // ServiceList = C.ConvertDataTable<Service>(dt);

            // List<Service> ServiceList = new List<Service>();
            // ServiceList= db.services.ToList()

            //IEnumerable<Service> list =
           // db.services;

            return View(db.services.ToList());
        }

        // GET: Service/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service service = db.services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // GET: Service/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Service/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Service service)
        {
            string q1 = $"insert into service values('{service.name}')";

            dbm.ExecuteNonQuery(q1);
            return RedirectToAction("Index");

        }

       

 
        // GET: Service/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service service = db.services.Find(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        // POST: Service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            service service = db.services.Find(id);
            db.services.Remove(service);
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
