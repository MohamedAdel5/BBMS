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
using BBMS.ViewModels;

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
            //var shift_manager = db.shift_manager.Include(s => s.login);
            //return View(shift_manager.ToList());
            ShiftManager inputShiftManager = (ShiftManager)TempData["inputShiftManager"];
            if (inputShiftManager == null)
            {
                return RedirectToAction("SignIn", "ShiftManager");
            }
            else
            {
                TempData["inputShiftManager"] = inputShiftManager;
                return View();
            }
        }




        //GET: Sing in page
        [Route("ShiftManager/SignIn")]
        public ActionResult SignIn()/*Empty function --> redirects the user to Sign In page*/
        {
            ShiftManager inputShiftManager = (ShiftManager)TempData["inputShiftManager"];
            if (inputShiftManager != null)
            {
                TempData["inputShiftManager"] = inputShiftManager;
                return RedirectToAction("Index", "ShiftManager");
            }
            else
            {
                return View();
            }
        }

        //POST: signIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(loginViewModel sm)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", sm.username);
            Parameters.Add("@password", sm.password);
            if (ModelState.IsValid)
            {
                DataTable inputSMTable = dbm.ExecuteReader_proc("checkShiftManager", Parameters);
                ShiftManager inputShiftManager;
                if (inputSMTable == null)
                {
                    return View(sm);
                }
                else
                {
                    string username = Convert.ToString(inputSMTable.Rows[0]["username"]);
                    string password = Convert.ToString(inputSMTable.Rows[0]["user_pass"].GetHashCode());
                    string name = Convert.ToString(inputSMTable.Rows[0]["name"]);
                    int hospital_id = Convert.ToInt32(inputSMTable.Rows[0]["hospital_id"]);
                    inputShiftManager = new ShiftManager()
                    {
                        username = username,
                        password = password,
                        name = name,
                        hospital_id = hospital_id
                    };
                    TempData["inputShiftManager"] = inputShiftManager;

                    return RedirectToAction("Index", "ShiftManager");
                }
            }
            else
            {
                return View(sm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            TempData.Remove("inputShiftManager");
            return RedirectToAction("SignIn", "ShiftManager");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GrantUser(FormCollection collection)
        {
            string username = Convert.ToString(collection["username"]);
            int points = Convert.ToInt32(collection["points"]);
            if(dbm.ExecuteReader_proc("checkUsernameIsUser", new Dictionary<string, object>() { { "@username", username} }) == null)
            {
                ViewBag.invalidUsername = true;
                return View("Index");
            }
            else
            {
                dbm.ExecuteReader_proc("GrantUserPoints", new Dictionary<string, object>() { { "@username", username }, { "@points", points } });
                ViewBag.successUsername = true;
                return View("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVolunteer(Volunteer v)
        {
            if(ModelState.IsValid)
            {
                if (dbm.ExecuteReader_proc("checkNationalID", new Dictionary<string, object>() { { "@n_id", v.national_id } }) != null)
                {
                    ViewBag.DuplicateNationalID = true;
                    return View("Index");
                }
                else
                {
                    Dictionary<string, object> Parameters = new Dictionary<string, object>();
                    Parameters.Add("@national_id", v.national_id);
                    Parameters.Add("@name", v.name);
                    Parameters.Add("@gender", v.gender);
                    Parameters.Add("@age", v.age);
                    Parameters.Add("@phone", v.phone);
                    Parameters.Add("@city", v.city);
                    Parameters.Add("@governorate", v.governorate);

                    if (dbm.ExecuteNonQuery_proc("insert_volunteer", Parameters) != 0)
                    {
                        ViewBag.successVolunteer = true;
                        return View("Index");
                    }
                    else
                    {
                        return Content("Fatal Error");
                    }
                }
            }
            else
            {
                return View("Index",v);
            }
            
        }
    }
}



//// GET: ShiftManager/Details/5
//public ActionResult Details(string id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    shift_manager shift_manager = db.shift_manager.Find(id);
//    if (shift_manager == null)
//    {
//        return HttpNotFound();
//    }
//    return View(shift_manager);
//}

//// GET: ShiftManager/Create
//public ActionResult Create()
//{
//    ViewBag.username = new SelectList(db.logins, "username", "user_type");
//    return View();
//}

//// POST: ShiftManager/Create
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Create( ShiftManager shift_manager)
//{
//    string q1 = $"insert into login (username,user_pass, user_type)"+$"VALUES ('{shift_manager.username}',HASHBYTES('SHA2_512','{shift_manager.password}'),'S')";
//    string q2 = $"INSERT INTO shift_manager(username,name)" +$"VALUES ('{shift_manager.username}','{shift_manager.name}')";


//    dbm.ExecuteNonQuery(q1);
//    dbm.ExecuteNonQuery(q2);
//    if (ModelState.IsValid)
//    {
//        //db.shift_manager.Add(shift_manager);
//       // db.SaveChanges();
//        return RedirectToAction("Index");
//    }

//    ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
//    return View(shift_manager);
//}

//// GET: ShiftManager/Edit/5
//public ActionResult Edit(string id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    shift_manager shift_manager = db.shift_manager.Find(id);
//    if (shift_manager == null)
//    {
//        return HttpNotFound();
//    }
//    ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
//    return View(shift_manager);
//}

//// POST: ShiftManager/Edit/5
//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
//[HttpPost]
//[ValidateAntiForgeryToken]
//public ActionResult Edit( shift_manager shift_manager)
//{
//    string q1 = $"update shift_manager set name='{shift_manager.name}' where username='{shift_manager.username}'";
//    string q2 = $"update login set user_pass=HASHBYTES('SHA2_512','{shift_manager.password}') where username='{shift_manager.username}'";
//    dbm.ExecuteNonQuery(q1);
//    dbm.ExecuteNonQuery(q2);

//    if (ModelState.IsValid)
//    {
//        //db.Entry(shift_manager).State = EntityState.Modified;
//        //db.SaveChanges();
//        return RedirectToAction("Index");
//    }
//    ViewBag.username = new SelectList(db.logins, "username", "user_type", shift_manager.username);
//    return View(shift_manager);
//}

//// GET: ShiftManager/Delete/5
//public ActionResult Delete(string id)
//{
//    if (id == null)
//    {
//        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
//    }
//    shift_manager shift_manager = db.shift_manager.Find(id);
//    if (shift_manager == null)
//    {
//        return HttpNotFound();
//    }
//    return View(shift_manager);
//}

//// POST: ShiftManager/Delete/5
//[HttpPost, ActionName("Delete")]
//[ValidateAntiForgeryToken]
//public ActionResult DeleteConfirmed(string id)
//{
//    shift_manager shift_manager = db.shift_manager.Find(id);
//    db.shift_manager.Remove(shift_manager);
//    db.SaveChanges();
//    return RedirectToAction("Index");
//}

//protected override void Dispose(bool disposing)
//{
//    if (disposing)
//    {
//        db.Dispose();
//    }
//    base.Dispose(disposing);
//}