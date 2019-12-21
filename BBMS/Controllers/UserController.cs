using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBMS.Models;
using BBMS.ViewModels;
using BBMS.db_access;
using System.Data;
using System.Web.Security;
using System.Data.SqlClient;

namespace BBMS.Controllers
{
    public class UserController : Controller
    {
        public DBManager dbm;
        public UserController()
        {
            dbm = new DBManager();
        }

        // GET: User (Profile Page)
        public ActionResult Index()
        {

            User inputUser = (User)TempData["inputUser"];
            if (inputUser == null)
            {
                return RedirectToAction("SignIn", "User");
            }
            
            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@national_id", inputUser.national_id);
            DataTable userDonations = dbm.ExecuteReader_proc("getUserDonations", Parameters1);

            Dictionary<string, object> Parameters2 = new Dictionary<string, object>();
            Parameters2.Add("@national_id", inputUser.national_id);
            DataTable userHealthInfo = dbm.ExecuteReader_proc("getUserHealthInfo", Parameters2);

            Dictionary<string, object> Parameters3 = new Dictionary<string, object>();
            Parameters3.Add("@national_id", inputUser.national_id);
            DataTable userServices = dbm.ExecuteReader_proc("getUserServices", Parameters3);

            Dictionary<string, object> Parameters4 = new Dictionary<string, object>();
            Parameters4.Add("@username", inputUser.username);
            DataTable notifications = dbm.ExecuteReader_proc("getUserNotifications", Parameters4);


            TempData["inputUser"] = inputUser;
            ViewBag.userDonations = userDonations;
            ViewBag.userHealthInfo = userHealthInfo;
            ViewBag.userServices = userServices;
            ViewBag.notifications = notifications;

            return View(inputUser);
        }

        //GET: Sign Up
        [Route("User/SignUp")]
        public ActionResult SignUp()
        {
            /*The viewBag is empty*/
            return View();
        }

        //POST: Sign Up
        /*This function checks if the inputs of the sign up form are valid. 
         * If they are valid it redirects the user to his homepage.
         * If they are not valid it leaves the user at the Sign up page with some errors.
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(User inputUser)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", inputUser.username);
            Parameters.Add("@user_pass", inputUser.password);
            Parameters.Add("@national_id", inputUser.national_id);
            Parameters.Add("@name", inputUser.name);
            Parameters.Add("@gender", inputUser.gender);
            Parameters.Add("@age", inputUser.age);
            Parameters.Add("@phone", inputUser.phone);
            Parameters.Add("@city", inputUser.city);
            Parameters.Add("@governorate", inputUser.governorate);
            
            if(ModelState.IsValid)
            {   
                if (dbm.ExecuteReader_proc("checkNationalID", new Dictionary<string, object> { { "@n_id", inputUser.national_id } }) != null)
                {
                    
                    ViewBag.DuplicateNationalID = true;
                    return View(inputUser);
                }
                if (dbm.ExecuteReader_proc("checkUsername", new Dictionary<string, object> { { "@username", inputUser.username } }) != null)
                {
                    
                    ViewBag.DuplicateUsername = true;
                    return View(inputUser);
                }
                if (dbm.ExecuteNonQuery_proc("insert_user", Parameters) != 0)
                {
                    TempData["inputUser"] = inputUser;
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    return Content("FATAL ERROR");
                }
            }
            else
            {
                return View(inputUser);
            }
        }
        //GET: Sing in page
        [Route("User/SignIn")]
        public ActionResult SignIn()
        {

            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();
            }
        }

        //POST: 
        /*
         * redirects the user to his profile pagee if the username and password are correct
         *else it throws an error and leaves you in the same page
         */
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(loginViewModel inputLogin)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", inputLogin.username);
            Parameters.Add("@password", inputLogin.password);

            if (ModelState.IsValid)
            {
                DataTable inputUserTable = dbm.ExecuteReader_proc("checkUser", Parameters);
                User inputUser;
                if (inputUserTable == null)
                {
                    return View(inputLogin);
                }
                else
                {
                    int age = Convert.ToInt32(inputUserTable.Rows[0]["age"]);
                    string city = Convert.ToString(inputUserTable.Rows[0]["city"]);
                    string governorate = Convert.ToString(inputUserTable.Rows[0]["governorate"]);
                    string username = Convert.ToString(inputUserTable.Rows[0]["username"]);
                    int points = Convert.ToInt32(inputUserTable.Rows[0]["points"]);
                    String blood_type = Convert.ToString(inputUserTable.Rows[0]["blood_type"]);
                    char gender = Convert.ToString(inputUserTable.Rows[0]["gender"])[0];
                    string name = Convert.ToString(inputUserTable.Rows[0]["name"]);
                    Int64 national_id = Convert.ToInt64(inputUserTable.Rows[0]["national_id"]);
                    string phone = Convert.ToString(inputUserTable.Rows[0]["phone"]);
                    string password = Convert.ToString(inputUserTable.Rows[0]["user_pass"].GetHashCode());
                   inputUser = new User()
                    {
                        age = age,
                        city = city,
                        governorate = governorate,
                        username = username,
                        points = points,
                        blood_type = blood_type,
                        gender = gender,
                        name = name,
                        national_id = national_id,
                        phone = phone,
                        password = password
                    };
                    TempData["inputUser"] = inputUser;
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                return View(inputLogin);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            TempData.Remove("inputUser");
            return RedirectToAction("Index", "Home");
        }
    }
}