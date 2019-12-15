using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBMS.Models;
using BBMS.ViewModels;

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
        // GET: Admin dashboard
        public ActionResult Index() /*This view also includes --> Hospitals Statistics*/
        {
            return View();
        }
        
        /*Retreives users statistics data from database and show it*/
        public ActionResult UsersStatistics()
        {
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
        public ActionResult SignUp(Admin a) 
        {
            /*Validates the sign up form data*/
            return View(); /*should retrun View(a) if the data is INVALID => this makes the user stay in the 
                            same page and receive errors if there were any
                            ELSE: it should redirect him to the index ACTION => ADMIN profile page*/
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
        public ActionResult SignIn(loginViewModel a)
        {
            return View();  /*If the input data from the sign in form is valid redirect to Index ACTION
                               ELSE: return View(a)*/
        }
}
}