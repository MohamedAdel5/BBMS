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
    public class ShiftManagerController : Controller
    {
        private EBBMSEntities db = new EBBMSEntities();

        public DBManager dbm;
        public ShiftManagerController()
        {
            dbm = new DBManager();
        }


        // GET: ShiftManager
        public ActionResult Index()
        {
            var shift_manager = db.shift_manager.Include(s => s.login);
            return View(shift_manager.ToList());
        }

        // GET: ShiftManager/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shift_manager shift_manager = db.shift_manager.Find(id);
            if (shift_manager == null)
            {
                return HttpNotFound();
            }
            return View(shift_manager);
        }

        // GET: ShiftManager/Create
        public ActionResult Create()
        {
            ViewBag.username = new SelectList(db.logins, "username", "user_type");
            return View();
        }

        // POST: ShiftManager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ShiftManager shift_manager)
        {
            string q1 = $"insert into login (username,user_pass, user_type)"+$"VALUES ('{shift_manager.username}',HASHBYTES('SHA2_512','{shift_manager.password}'),'S')";
            string q2 = $"INSERT INTO shift_manager(username,name)" +$"VALUES ('{shift_manager.username}','{shift_manager.name}')";


            dbm.ExecuteNonQuery(q1);
            dbm.ExecuteNonQuery(q2);
            if (ModelState.IsValid)
            {
                //db.shift_manager.Add(shift_manager);
               // db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
            return View(shift_manager);
        }

        // GET: ShiftManager/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shift_manager shift_manager = db.shift_manager.Find(id);
            if (shift_manager == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
            return View(shift_manager);
        }

        // POST: ShiftManager/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( shift_manager shift_manager)
        {
            string q1 = $"update shift_manager set name='{shift_manager.name}' where username='{shift_manager.username}'";
            string q2 = $"update login set user_pass=HASHBYTES('SHA2_512','{shift_manager.password}') where username='{shift_manager.username}'";
            dbm.ExecuteNonQuery(q1);
            dbm.ExecuteNonQuery(q2);

            if (ModelState.IsValid)
            {
                //db.Entry(shift_manager).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
            return View(shift_manager);
        }

        // GET: ShiftManager/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            shift_manager shift_manager = db.shift_manager.Find(id);
            if (shift_manager == null)
            {
                return HttpNotFound();
            }
            return View(shift_manager);
        }

        // POST: ShiftManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            shift_manager shift_manager = db.shift_manager.Find(id);
            db.shift_manager.Remove(shift_manager);
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
