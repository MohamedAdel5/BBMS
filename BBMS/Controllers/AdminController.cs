using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBMS.Models;
using BBMS.ViewModels;
using BBMS.db_access;
using System.Data;
using System.Data.SqlClient;

namespace BBMS.Controllers
{
    public class AdminController : Controller
    {
        /*This function receives the Admin model and loads any the needed data for his profile
        from the database if there is any needed data. Then it passes all the needed 
        data to the View (Admin dashboard)
        */
        /*For now the admin dashboard is the same for all admins 
         * (all of them access the same dashboard with different usernames and passwords)
             So we don't need the Admin model as a parameter here
         */
        public DBManager dbm;
        public AdminController()
        {
            dbm = new DBManager();
        }
        // GET: Admin dashboard
        public ActionResult Index() /*This view also includes --> Hospitals Statistics*/
        {
            Admin inputAdmin = (Admin)TempData["inputAdmin"];

            DataTable bloodBagsStatistics = dbm.ExecuteReader_proc("getBBagsNums", null);

            Int64 AP = 0;
            Int64 AN = 0;
            Int64 BP = 0;
            Int64 BN = 0;
            Int64 ABP = 0;
            Int64 ABN = 0;
            Int64 OP = 0;
            Int64 ON = 0;

            foreach (DataRow row in bloodBagsStatistics.Rows)
            {
                if (Convert.ToString(row["blood_type"]) == "A+")
                {
                    AP = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "A-")
                {
                    AN = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "B+")
                {
                    BP = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "B-")
                {
                    BN = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "AB+")
                {
                    ABP = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "AB-")
                {
                    ABN = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "O+")
                {
                    OP = Convert.ToInt64(row["Num"]);
                }
                else if (Convert.ToString(row["blood_type"]) == "O-")
                {
                    ON = Convert.ToInt64(row["Num"]);
                }
            }

            ViewBag.AP = AP;
            ViewBag.AN = AN;
            ViewBag.BP = BP;
            ViewBag.BN = BN;
            ViewBag.ABP = ABP;
            ViewBag.ABN = ABN;
            ViewBag.OP = OP;
            ViewBag.ON = ON;

            DataTable hospitalsStatistics = dbm.ExecuteReader_proc("mostHospitalUsed", null);

            string[] hospitals = new string[10];
            Int64[] usingCount = new Int64[10];

            for (int i = 0; i < 10; ++i)
            {
                hospitals[i] = " ";
                usingCount[i] = 0;
            }

            for (int i = 0; i < hospitalsStatistics.Rows.Count; ++i)
            {
                hospitals[i] = Convert.ToString(hospitalsStatistics.Rows[i]["hospital_name"]);
                if (hospitalsStatistics.Rows[i]["num"] != DBNull.Value)
                {
                    usingCount[i] = Convert.ToInt64(hospitalsStatistics.Rows[i]["num"]);
                }
            }

            ViewBag.topHospitalsCount = hospitalsStatistics.Rows.Count;

            ViewBag.hospitals = hospitals;
            ViewBag.usingCount = usingCount;

            return View(inputAdmin);
        }
        
        /*Retreives users statistics data from database and show it*/
        public ActionResult UsersStatistics()
        {

            DataTable topUsers = dbm.ExecuteReader_proc("getTopUsers", null);
            DataTable topUserServices = dbm.ExecuteReader_proc("topUsersUseService", null);

            string[] users = new string[10];
            Int64[] usersDon = new Int64[10];

            string[] usersService = new string[10];
            Int64[] usersCountServ = new Int64[10];

            for (int i = 0; i < 10; ++i)
            {
                users[i] = " ";
                usersService[i] = " ";
                usersDon[i] = 0;
                usersCountServ[i] = 0;
            }

            for (int i = 0; i < topUsers.Rows.Count; ++i)
            {
                users[i] = Convert.ToString(topUsers.Rows[i]["name"]);
                if (topUsers.Rows[i]["donation_count"] != DBNull.Value)
                {
                    usersDon[i] = Convert.ToInt64(topUsers.Rows[i]["donation_count"]);
                }
            }

            ViewBag.topDonatorsCount = topUsers.Rows.Count;

            for (int i = 0; i < topUserServices.Rows.Count; ++i)
            {
                usersService[i] = Convert.ToString(topUserServices.Rows[i]["name"]);
                if (topUserServices.Rows[i]["num"] != DBNull.Value)
                {
                    usersCountServ[i] = Convert.ToInt64(topUserServices.Rows[i]["num"]);
                }
            }

            ViewBag.topUserServiceCount = topUserServices.Rows.Count;

            ViewBag.users = users;
            ViewBag.donsCounts = usersDon;

            ViewBag.usersService = usersService;
            ViewBag.usersCountServ = usersCountServ;

            return View();
        }

        //GET: Sign Up
        [Route("Admin/SignUp")]
        public ActionResult SignUp() /*Empty function --> redirects the user to Sign Up page*/
        {
            /*The viewBag is empty*/
            return View();
        }

        //POST: signUp;;
        [HttpPost]
        public ActionResult SignUp(Admin admin) 
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", admin.username);
            Parameters.Add("@user_pass", admin.password);

            if (ModelState.IsValid)
            {
                if (dbm.ExecuteReader_proc("checkUsername", new Dictionary<string, object> { { "@username", admin.username } }) != null)
                {

                    ViewBag.DuplicateUsername = true;
                    return View(admin);
                }
                if (dbm.ExecuteNonQuery_proc("insert_admin", Parameters) != 0)
                {
                    TempData["inputAdmin"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    return Content("FATAL ERROR");
                }
            }
            else
            {
                return View(admin);
            }
        }


        //GET: Sing in page
        [Route("Admin/SignIn")]
        public ActionResult SignIn()/*Empty function --> redirects the user to Sign In page*/
        {
            /*The viewBag is empty*/
            return View();
        }

        //POST: signIn
        [HttpPost]
        public ActionResult SignIn(loginViewModel admin)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", admin.username);
            Parameters.Add("@password", admin.password);

            if (ModelState.IsValid)
            {
                DataTable inputAdminTable = dbm.ExecuteReader_proc("checkAdmin", Parameters);
                Admin inputAdmin;
                if (inputAdminTable == null)
                {
                    return View(admin);
                }
                else
                {
                    string username = Convert.ToString(inputAdminTable.Rows[0]["username"]);
                    string password = Convert.ToString(inputAdminTable.Rows[0]["user_pass"].GetHashCode());
                    inputAdmin = new Admin()
                    {
                        username = username,
                        password = password
                    };
                    TempData["inputAdmin"] = inputAdmin;
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return View(admin);
            }
        }
}
}