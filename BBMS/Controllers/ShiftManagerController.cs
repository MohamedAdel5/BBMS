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
            ShiftManager inputShiftManager = (ShiftManager)TempData["inputShiftManager"];
            if (inputShiftManager == null)
            {
                return RedirectToAction("SignIn", "ShiftManager");
            }
            else
            {
                TempData["inputShiftManager"] = inputShiftManager;
                ViewBag.passSuccess = Convert.ToBoolean(TempData["passSuccess"]);
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
            
            if(String.IsNullOrEmpty(username) || String.IsNullOrEmpty(collection["points"]))
            {
                ViewBag.InvalidInput = true;
                return View("Index");
            }
            int points = Convert.ToInt32(collection["points"]);
            if (dbm.ExecuteReader_proc("checkUsernameIsUser", new Dictionary<string, object>() { { "@username", username} }) == null)
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
        [HttpPost]
        public ActionResult ChangePassword(string pass)
        {
            ShiftManager inputShiftManager = (ShiftManager)TempData["inputShiftManager"];
            if (inputShiftManager != null)
            {
                TempData["inputShiftManager"] = inputShiftManager;
            }
            else
            {
                return RedirectToAction("Index", "ShiftManager");
            }
            if (String.IsNullOrEmpty(pass))
            {
                return RedirectToAction("Index", "ShiftManager");
            }
            if (pass.Length > 30)
            {
                return RedirectToAction("Index", "ShiftManager");
            }
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", inputShiftManager.username);
            Parameters.Add("@password", pass);
            if (dbm.ExecuteNonQuery_proc("changePassword", Parameters) != 0)
            {
                TempData["passSuccess"] = true;
            }
            return RedirectToAction("Index", "ShiftManager");
        }
    }
}

