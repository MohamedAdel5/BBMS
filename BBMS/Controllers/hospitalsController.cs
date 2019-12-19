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
            /*var hospitals = db.hospitals.Include(h => h.login);
            return View(hospitals.ToList());*/
            return View();

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
            //string q1 = $"INSERT INTO login (username,user_pass, user_type) " + $"Values ('{hospital.username}', HASHBYTES('SHA2_512','{hospital.password}'),'H')";
            //    dbm.ExecuteNonQuery(q1);
            //string q2 = "INSERT INTO Hospital (username,hospital_name, phone, City, governorate)"+$"Values('{hospital.username}','{hospital.hospital_name}','{hospital.phone}','{hospital.city}','{hospital.governorate}')";

            //dbm.ExecuteNonQuery(q2);
            //if (ModelState.IsValid)
            //{

            //    return RedirectToAction("Index", "Admin");
            //}

            //ViewBag.username = new SelectList(db.logins, "username", "user_type", hospital.username);
            //return View(hospital);
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", hospital.username);
            Parameters.Add("@user_pass", hospital.password);
            Parameters.Add("@hospital_name", hospital.hospital_name);
            Parameters.Add("@phone", Convert.ToInt64(hospital.phone));
            Parameters.Add("@city", hospital.city);
            Parameters.Add("@governorate", hospital.governorate);
             
            if (ModelState.IsValid)
            {
                if (dbm.ExecuteNonQuery_proc("insert_hospital", Parameters) != 0)
                {
                    
                    return RedirectToAction("Create", "hospitals");
                }
                else
                {
                    return Content("FATAL ERROR");
                }
            }
            else
            {
                return View(hospital);
            }
            
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

        //// GET: hospitals/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    hospital hospital = db.hospitals.Find(id);
        //    if (hospital == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hospital);
        //}
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // POST: hospitals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //hospital hospital = db.hospitals.Find(id);
            //db.hospitals.Remove(hospital);
            //db.SaveChanges();
            if (dbm.ExecuteNonQuery_proc("deleteHospital", new Dictionary<string, object>() { { "@h_id", id } }) != 0)
            {
                return RedirectToAction("RemoveHospital");
            }
            else
            {
                return Content("Fatal error");
            }
        }

        //GET: remove hospital View
        public ActionResult RemoveHospital()
        {
          
            /*Get all hospitals names and ids and send them to the view through the view bag*/
            return View();
        }

        //GET: remove hospital View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveHospital(string searchString)
        {
            /*Retreieve the selected hospital id and redirect*/

            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable hospitals = dbm.ExecuteReader_proc("getHospitals", null /*-> no parameters*/);  /*Gets all hospitals*/
                DataTable hospitalsFilter = new DataTable();    /*will contain the hospitals that match the string*/
                hospitalsFilter.Columns.Add(new DataColumn("hospital_name", typeof(string)));
                hospitalsFilter.Columns.Add(new DataColumn("hospital_id", typeof(int)));

                foreach (DataRow row in hospitals.Rows)
                {
                    if(Convert.ToString(row["hospital_name"]).ToLower().Contains(searchString.ToLower()))
                    {
                        DataRow r = hospitalsFilter.NewRow();
                        r["hospital_name"] = Convert.ToString(row["hospital_name"]);
                        r["hospital_id"] = Convert.ToInt32(row["hospital_id"]);
                        hospitalsFilter.Rows.Add(r);
                    }
                    
                }
                if(hospitalsFilter.Rows.Count != 0)
                    ViewBag.hospitalsFilter = hospitalsFilter;

            }
            return View();
        }
    }
}
