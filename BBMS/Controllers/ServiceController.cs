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
            Admin inputAdmin = (Admin)TempData["inputAdmin"];
            if (inputAdmin != null)
            {
                TempData["inputAdmin"] = inputAdmin;
            }
            else
            {
                return RedirectToAction("SignIn", "Admin");
            }
            return View();
        }

        // POST: Service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( string service_name)
        {
            //string q1 = $"insert into service values('{service.name}')";

            //dbm.ExecuteNonQuery(q1);
            //return RedirectToAction("Index");
            Admin inputAdmin = (Admin)TempData["inputAdmin"];
            if (inputAdmin != null)
            {
                TempData["inputAdmin"] = inputAdmin;
            }
            else
            {
                return RedirectToAction("SignIn", "Admin");
            }
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@name", service_name);
            if (dbm.ExecuteNonQuery_proc("insert_service", Parameters) != 0)
            {
                /*The service is inserted --> go back to The dashboard*/
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Failure = true;
                return View();
            }

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

        //GET: show services and a remove button
        public ActionResult ShowRemoveServices()
        {
            Admin inputAdmin = (Admin)TempData["inputAdmin"];
            if (inputAdmin != null)
            {
                TempData["inputAdmin"] = inputAdmin;
            }
            else
            {
                return RedirectToAction("SignIn", "Admin");
            }
            DataTable services = dbm.ExecuteReader_proc("getServicesNamesAndIDs", null /*-> no parameters*/);  /*Gets all Services*/

            ViewBag.services = services;
            return View();
        }

        //POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveService(int id)
        {
            Admin inputAdmin = (Admin)TempData["inputAdmin"];
            if (inputAdmin != null)
            {
                TempData["inputAdmin"] = inputAdmin;
            }
            else
            {
                return RedirectToAction("SignIn", "Admin");
            }

            if (dbm.ExecuteNonQuery_proc("deleteService", new Dictionary<string, object>() { { "@service_id", id } }) != 0)
            {
                return RedirectToAction("ShowRemoveServices");
            }
            else
            {
                return Content("Fatal error");
            }
        }
    }
}
