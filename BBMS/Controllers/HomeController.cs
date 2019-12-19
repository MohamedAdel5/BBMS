using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BBMS.Models;
using BBMS.db_access;
using System.Data;

namespace BBMS.Controllers
{
    public class HomeController : Controller
    {
        DBManager dbm;

        public HomeController()
        {
            dbm = new DBManager();
        }

        public ActionResult Index()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }


            return View();
        }

        public ActionResult About()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }
            ViewBag.Message = "This is our BBMS Project.";

            return View();
        }

        public ActionResult Contact()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }
            ViewBag.Message = "This is our Contact page.";

            return View();
        }
        /*-------------------------------------------------------------------------------------------------*/

        public ActionResult ShowProvidedServices()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }
            DataTable services = dbm.ExecuteReader_proc("getServices", null /*-> no parameters*/);  /*Gets all Services*/
            
            ViewBag.services = services;
            return View();
        }

        [HttpPost]
        public ActionResult SearchProvidedServices(string searchString)
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;
            }
            DataTable services = dbm.ExecuteReader_proc("getServices", null /*-> no parameters*/);  /*Gets all services*/

            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable servicesFilter = new DataTable();    /*will contain the hospitals that match the string*/
                servicesFilter.Columns.Add(new DataColumn("Service name", typeof(string)));
                servicesFilter.Columns.Add(new DataColumn("Provided By Hospital", typeof(string)));
                servicesFilter.Columns.Add(new DataColumn("Service Value", typeof(int)));

                foreach (DataRow row in services.Rows)
                {
                    if (Convert.ToString(row["name"]).ToLower().Contains(searchString.ToLower()))
                    {
                        DataRow r = servicesFilter.NewRow();
                        r["Service name"] = Convert.ToString(row["name"]);
                        r["Provided By Hospital"] = Convert.ToString(row["hospital_name"]);
                        r["Service Value"] = Convert.ToInt32(row["value"]);
                        servicesFilter.Rows.Add(r);
                    }

                }
                if (servicesFilter.Rows.Count != 0)
                    ViewBag.servicesFilter = servicesFilter;

            }
            ViewBag.services = services;

            return View("ShowProvidedServices");
        }
        /*-------------------------------------------------------------------------------------------------*/

        public ActionResult ShowHospitals()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }
            DataTable hospitals = dbm.ExecuteReader_proc("getHospitals", null /*-> no parameters*/);  /*Gets all hospitals*/
            hospitals.Columns.Remove("username");
            hospitals.Columns.Remove("hospital_id");

            ViewBag.hospitals = hospitals;
            return View();
        }

        [HttpPost]
        public ActionResult SearchHospitals(string searchString)
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;
            }
            DataTable hospitals = dbm.ExecuteReader_proc("getHospitals", null /*-> no parameters*/);  /*Gets all hospitals*/
            hospitals.Columns.Remove("username");
            hospitals.Columns.Remove("hospital_id");

            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable hospitalsFilter = new DataTable();    /*will contain the hospitals that match the string*/
                hospitalsFilter.Columns.Add(new DataColumn("Hospital Name", typeof(string)));
                hospitalsFilter.Columns.Add(new DataColumn("Phone number", typeof(Int64)));
                hospitalsFilter.Columns.Add(new DataColumn("Governorate", typeof(string)));
                hospitalsFilter.Columns.Add(new DataColumn("City", typeof(string)));

                foreach (DataRow row in hospitals.Rows)
                {
                    if (Convert.ToString(row["hospital_name"]).ToLower().Contains(searchString.ToLower()))
                    {
                        DataRow r = hospitalsFilter.NewRow();
                        r["Hospital Name"] = Convert.ToString(row["hospital_name"]);
                        r["Phone number"] = Convert.ToInt64(row["phone"]);
                        r["Governorate"] = Convert.ToString(row["governorate"]);
                        r["City"] = Convert.ToString(row["city"]);
                        hospitalsFilter.Rows.Add(r);
                    }

                }
                if (hospitalsFilter.Rows.Count != 0)
                    ViewBag.hospitalsFilter = hospitalsFilter;

            }
            ViewBag.hospitals = hospitals;

            return View("ShowHospitals");
        }

        /*-------------------------------------------------------------------------------------------------*/
        public ActionResult ShowBloodCamps()
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;

            }
            DataTable bloodCamps = dbm.ExecuteReader_proc("getBloodCampsAndShifts", null /*-> no parameters*/);  /*Gets all blood camps*/

            bloodCamps.Columns.Add(new DataColumn("shift_date2", typeof(string)));
            foreach (DataRow row in bloodCamps.Rows)
            {
                
                DateTime d = Convert.ToDateTime(row["shift_date"]);
                if (d < DateTime.Now)
                {
                    row.Delete();
                    continue;
                }
                string year = d.Year.ToString();
                string month = (d.Month < 10)? "0"+ d.Month: "" + d.Month;
                string day = (d.Day < 10) ? "0" + d.Day : "" + d.Day;
                row["shift_date2"] = $"{year}-{month}-{day}";

            }
            bloodCamps.Columns.Remove("shift_date");
            bloodCamps.AcceptChanges();

            bloodCamps.Columns["blood_camp_id"].ColumnName = "Blood Camp ID";
            bloodCamps.Columns["hospital_name"].ColumnName = "Hospital Name";
            bloodCamps.Columns["shift_date2"].ColumnName = "Shift date";
            bloodCamps.Columns["start_hour"].ColumnName = "Start time";
            bloodCamps.Columns["finish_hour"].ColumnName = "Finish time";
            bloodCamps.Columns["governorate"].ColumnName = "Governorate";
            bloodCamps.Columns["city"].ColumnName = "City";
            bloodCamps.Columns["name"].ColumnName = "Shift manager name";
            bloodCamps.Columns["driver_name"].ColumnName = "Driver name";
            
            ViewBag.bloodCamps = bloodCamps;
            return View();
        }

        [HttpPost]
        public ActionResult SearchBloodCampsByLoc(string searchString)
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;
            }
            DataTable bloodCamps = dbm.ExecuteReader_proc("getBloodCampsAndShifts", null /*-> no parameters*/);  /*Gets all blood camps*/
            bloodCamps.Columns.Add(new DataColumn("shift_date2", typeof(string)));
            foreach (DataRow row in bloodCamps.Rows)
            {

                DateTime d = Convert.ToDateTime(row["shift_date"]);
                if (d < DateTime.Now)
                {
                    row.Delete();
                    continue;
                }
                string year = d.Year.ToString();
                string month = (d.Month < 10) ? "0" + d.Month : "" + d.Month;
                string day = (d.Day < 10) ? "0" + d.Day : "" + d.Day;
                row["shift_date2"] = $"{year}-{month}-{day}";

            }
            bloodCamps.Columns.Remove("shift_date");
            bloodCamps.AcceptChanges();


            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable bloodCampsFilter = new DataTable();    /*will contain the bloodCamps that match the string*/
                bloodCampsFilter.Columns.Add(new DataColumn("Blood Camp ID", typeof(int)));
                bloodCampsFilter.Columns.Add(new DataColumn("Hospital Name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Start time", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Finish time", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Governorate", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("City", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Shift manager name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Driver name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Shift date", typeof(string)));


                foreach (DataRow row in bloodCamps.Rows)
                {
                    if (
                        Convert.ToString(row["governorate"]).ToLower().Contains(searchString.ToLower())
                        ||
                        Convert.ToString(row["city"]).ToLower().Contains(searchString.ToLower())
                        )
                    {
                        DataRow r = bloodCampsFilter.NewRow();
                        r["Blood Camp ID"] =         Convert.ToInt32(row["blood_camp_id"]);
                        r["Hospital Name"] =        Convert.ToString(row["hospital_name"]);
                        r["Start time"] =           Convert.ToString(row["start_hour"]);
                        r["Finish time"] =          Convert.ToString(row["finish_hour"]);
                        r["Governorate"] =          Convert.ToString(row["governorate"]);
                        r["City"] =                 Convert.ToString(row["city"]);
                        r["Shift manager name"] =   Convert.ToString(row["name"]);
                        r["Driver name"] =          Convert.ToString(row["driver_name"]);
                        r["Shift date"] = Convert.ToString(row["shift_date2"]);

                        bloodCampsFilter.Rows.Add(r);
                    }

                }
                if (bloodCampsFilter.Rows.Count != 0)
                    ViewBag.bloodCampsFilter = bloodCampsFilter;

            }
            bloodCamps.Columns["blood_camp_id"].ColumnName = "Blood Camp ID";
            bloodCamps.Columns["hospital_name"].ColumnName = "Hospital Name";
            bloodCamps.Columns["start_hour"].ColumnName = "Start time";
            bloodCamps.Columns["finish_hour"].ColumnName = "Finish time";
            bloodCamps.Columns["governorate"].ColumnName = "Governorate";
            bloodCamps.Columns["city"].ColumnName = "City";
            bloodCamps.Columns["name"].ColumnName = "Shift manager name";
            bloodCamps.Columns["driver_name"].ColumnName = "Driver name";
            bloodCamps.Columns["shift_date2"].ColumnName = "Shift date";


            ViewBag.bloodCamps = bloodCamps;

            return View("ShowBloodCamps");
        }

        [HttpPost]
        public ActionResult SearchBloodCampsByDate(string searchString)
        {
            User inputUser = (User)TempData["inputUser"];
            if (inputUser != null)
            {
                TempData["inputUser"] = inputUser;
                ViewBag.userLoggedIn = true;
            }
            DataTable bloodCamps = dbm.ExecuteReader_proc("getBloodCampsAndShifts", null /*-> no parameters*/);  /*Gets all blood camps*/
            bloodCamps.Columns.Add(new DataColumn("shift_date2", typeof(string)));
            foreach (DataRow row in bloodCamps.Rows)
            {

                DateTime d = Convert.ToDateTime(row["shift_date"]);
                if (d < DateTime.Now)
                {
                    row.Delete();
                    continue;
                }
                string year = d.Year.ToString();
                string month = (d.Month < 10) ? "0" + d.Month : "" + d.Month;
                string day = (d.Day < 10) ? "0" + d.Day : "" + d.Day;
                row["shift_date2"] = $"{year}-{month}-{day}";

            }
            bloodCamps.Columns.Remove("shift_date");
            bloodCamps.AcceptChanges();

            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable bloodCampsFilter = new DataTable();    /*will contain the bloodCamps that match the string*/
                bloodCampsFilter.Columns.Add(new DataColumn("Blood Camp ID", typeof(int)));
                bloodCampsFilter.Columns.Add(new DataColumn("Hospital Name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Start time", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Finish time", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Governorate", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("City", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Shift manager name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Driver name", typeof(string)));
                bloodCampsFilter.Columns.Add(new DataColumn("Shift date", typeof(string)));



                foreach (DataRow row in bloodCamps.Rows)
                {
                    DateTime shiftDate = Convert.ToDateTime(row["shift_date2"]);
                    DateTime searchDate = Convert.ToDateTime(searchString);
                    if (shiftDate.Date == searchDate.Date)
                    {
                        DataRow r = bloodCampsFilter.NewRow();
                        r["Blood Camp ID"] = Convert.ToInt32(row["blood_camp_id"]);
                        r["Hospital Name"] = Convert.ToString(row["hospital_name"]);
                        r["Start time"] = Convert.ToString(row["start_hour"]);
                        r["Finish time"] = Convert.ToString(row["finish_hour"]);
                        r["Governorate"] = Convert.ToString(row["governorate"]);
                        r["City"] = Convert.ToString(row["city"]);
                        r["Shift manager name"] = Convert.ToString(row["name"]);
                        r["Driver name"] = Convert.ToString(row["driver_name"]);
                        r["Shift date"] = Convert.ToString(row["shift_date2"]);


                        bloodCampsFilter.Rows.Add(r);
                    }

                }
                if (bloodCampsFilter.Rows.Count != 0)
                    ViewBag.bloodCampsFilter = bloodCampsFilter;

            }
            bloodCamps.Columns["blood_camp_id"].ColumnName = "Blood Camp ID";
            bloodCamps.Columns["hospital_name"].ColumnName = "Hospital Name";
            bloodCamps.Columns["start_hour"].ColumnName = "Start time";
            bloodCamps.Columns["finish_hour"].ColumnName = "Finish time";
            bloodCamps.Columns["governorate"].ColumnName = "Governorate";
            bloodCamps.Columns["city"].ColumnName = "City";
            bloodCamps.Columns["name"].ColumnName = "Shift manager name";
            bloodCamps.Columns["driver_name"].ColumnName = "Driver name";
            bloodCamps.Columns["shift_date2"].ColumnName = "Shift date";

            ViewBag.bloodCamps = bloodCamps;

            return View("ShowBloodCamps");
        }
    }
}