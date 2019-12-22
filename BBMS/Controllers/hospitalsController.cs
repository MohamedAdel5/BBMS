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
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }
            ViewBag.passSuccess = Convert.ToBoolean(TempData["passSuccess"]);
            ViewBag.BloodTypes = new List<object> { "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            return View(inputHospital);

        }

        //SignUp
        // GET: hospitals/Create
        public ActionResult Create()
        {
            return View();
        }

        //SignUp
        // POST: hospitals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Hospital hospital)
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            TempData.Remove("inputHospital");
            return RedirectToAction("SignIn", "Hospitals");
        }

        // POST: hospitals/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
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
            Admin inputAdmin = (Admin)TempData["inputAdmin"];
            if (inputAdmin != null)
            {
                TempData["inputAdmin"] = inputAdmin;
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Admin");
            }
        }

        //GET: remove hospital View
        [HttpPost]
        public ActionResult RemoveHospital(string searchString)
        {
            /*Retreieve the selected hospital id and redirect*/

            if (!String.IsNullOrEmpty(searchString))
            {
                DataTable hospitals = dbm.ExecuteReader_proc("getHospitals", null /*-> no parameters*/);  /*Gets all hospitals*/
                if(hospitals == null)
                {
                    return View();
                }
                DataTable hospitalsFilter = new DataTable();    /*will contain the hospitals that match the string*/
                hospitalsFilter.Columns.Add(new DataColumn("hospital_name", typeof(string)));
                hospitalsFilter.Columns.Add(new DataColumn("hospital_id", typeof(int)));

                foreach (DataRow row in hospitals.Rows)
                {
                    if(Convert.ToString(row["hospital_name"]).Contains(searchString))
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



        //GET: Sing in page
        [Route("hospitals/SignIn")]
        public ActionResult SignIn()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
                return RedirectToAction("Index", "Hospitals");
            }
            else
            {
                return View();
            }
            /*The viewBag is empty*/
            return View();
        }

        //POST: 
        /*
         * redirects the hospital admin to his dashboard if the username and password are correct
         *else it throws an error and leaves him in the same page
         */
        [HttpPost]
        public ActionResult SignIn(loginViewModel inputLogin)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", inputLogin.username);
            Parameters.Add("@password", inputLogin.password);

            if (ModelState.IsValid)
            {
                DataTable inputHospitalTable = dbm.ExecuteReader_proc("checkHospital", Parameters);
                Hospital inputHospital;
                if (inputHospitalTable == null)
                {
                    return View(inputLogin);
                }
                else
                {
                    int hospital_id = Convert.ToInt32(inputHospitalTable.Rows[0]["hospital_id"]);
                    string hospital_name = Convert.ToString(inputHospitalTable.Rows[0]["hospital_name"]);
                    string city = Convert.ToString(inputHospitalTable.Rows[0]["city"]);
                    string governorate = Convert.ToString(inputHospitalTable.Rows[0]["governorate"]);
                    string username = Convert.ToString(inputHospitalTable.Rows[0]["username"]);
                    string phone = Convert.ToString(inputHospitalTable.Rows[0]["phone"]);
                    string password = Convert.ToString(inputHospitalTable.Rows[0]["user_pass"].GetHashCode());
                    inputHospital = new Hospital()
                    {
                        hospital_id = hospital_id,
                        hospital_name = hospital_name,
                        city = city,
                        governorate = governorate,
                        username = username,
                        phone = phone,
                        password = password
                    };
                    TempData["inputHospital"] = inputHospital;
                    return RedirectToAction("Index", "hospitals");
                }
            }
            else
            {
                return View(inputLogin);
            }

        }


        public ActionResult Services()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable ProvidedServices = dbm.ExecuteReader_proc("HospitalProvideServices", Parameters1);
            DataTable NotProvidedServices = dbm.ExecuteReader_proc("HospitalNotProvideServices", Parameters1);
            ViewBag.ProvidedServicesDT = ProvidedServices;
            if (NotProvidedServices != null)

            ViewBag.NotProvidedServices = NotProvidedServices.AsEnumerable().Select(row => new service
            {
                service_id = Convert.ToInt32(row["service_id"]),
                name = (row["name"]).ToString()
            });


            if (ProvidedServices != null)

                ViewBag.ProvidedServices = ProvidedServices.AsEnumerable().Select(row => new service
            {
                service_id = Convert.ToInt32(row["service_id"]),
                name = (row["name"]).ToString()
            });
             
            

            return View();

        }

        [HttpPost]
        public ActionResult AddService(Service srv)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@service_id", srv.service_id);
            Parameters1.Add("@value", srv.value);

            dbm.ExecuteNonQuery_proc("AddServiceToHospital", Parameters1);


            return Redirect("Services");
        }

        [HttpPost]
        public ActionResult RemoveService(Service srv)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@service_id", srv.service_id);

            dbm.ExecuteNonQuery_proc("RemoveServiceFromHospital", Parameters1);


            return Redirect("Services");
        }






        public ActionResult BloodCamps()
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            ViewBag.BloodCampsDetails = dbm.ExecuteReader_proc("getBloodCampsDetails", Parameters1);

            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });


            return View();

        }

        public ActionResult AddCamp(BloodCamp camp)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@driver_name", camp.driver_name);

            dbm.ExecuteNonQuery_proc("AddBloodCamp", Parameters1);


            return Redirect("BloodCamps");
        }

        [HttpPost]
        public ActionResult RemoveCamp(BloodCamp camp)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_camp_id", camp.blood_camp_id);

            dbm.ExecuteNonQuery_proc("RemoveBloodCamp", Parameters1);

            TempData["inputHospital"] = inputHospital;


            return Redirect("BloodCamps");
        }




        public ActionResult RemoveShift()
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)
                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });    

            return View();

        }

        [HttpPost]
        public ActionResult RemoveShift(Shift shift)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@blood_camp_id", shift.blood_camp_id);

            ViewBag.shifts = dbm.ExecuteReader_proc("getCampShifts", Parameters1);


            Parameters1.Clear();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)
                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });

            return View();
        }



        public ActionResult RemoveShiftConfirmed(int blood_camp_id, string shift_date)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@blood_camp_id", blood_camp_id);
            Parameters1.Add("@shift_date", shift_date);
            

            if (dbm.ExecuteNonQuery_proc("RemoveShift", Parameters1) != 0)
            {
                return RedirectToAction("RemoveShift");
            }
            else
            {
                return Content("Fatal error");
            }

        }


        public ActionResult AddShift()
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });


            return View();


        }

        [HttpPost]
        public ActionResult AddShift(Shift shift, string start, string end, string shift_date)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            start += ":00";
            end += ":00";

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@hospital_id", inputHospital.hospital_id);

            
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters);

            if (BloodCamps != null)
            {

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });

                DataTable myShiftManagers = dbm.ExecuteReader_proc("getShiftManagers", Parameters);
                if (!(myShiftManagers).AsEnumerable().Any(row => shift.shift_manager_username == row.Field<String>("username")))
                {
                    return View();
                }
                Parameters.Clear();

                Parameters.Add("@blood_camp_id", shift.blood_camp_id);
                Parameters.Add("@shift_date", shift_date);
                Parameters.Add("@shift_manager_username", shift.shift_manager_username);
                Parameters.Add("@start_hour", start);
                Parameters.Add("@finish_hour", end);
                Parameters.Add("@city", shift.city);
                Parameters.Add("@governorate", shift.governorate);

                ViewBag.added = (dbm.ExecuteNonQuery_proc("insert_shift", Parameters) != 0);
            }


            return View();   
        }




        public ActionResult AddShiftManager() {


            return View();
        }

        [HttpPost]
        public ActionResult AddShiftManager(ShiftManager mgr)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@username", mgr.username);
            Parameters1.Add("@user_pass", mgr.password);
            Parameters1.Add("@name", mgr.name);
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            ViewBag.added = (dbm.ExecuteNonQuery_proc("insert_shift_manager", Parameters1) != 0);

            return View();


        }


        public ActionResult RemoveShiftManager()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            ViewBag.managers = dbm.ExecuteReader_proc("getShiftManagers", Parameters1);

            return View();
        }

        [HttpPost]
        public ActionResult RemoveShiftManager(string mgr)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable managers = dbm.ExecuteReader_proc("getShiftManagers", Parameters1);
            if(!String.IsNullOrEmpty(mgr) && managers != null)
            {
                ViewBag.removed = (managers).AsEnumerable().Any(row => mgr == row.Field<String>("username"));

                if (ViewBag.removed == true)
                {
                    Parameters1.Clear();
                    Parameters1.Add("@username", mgr);
                    ViewBag.removed = dbm.ExecuteNonQuery_proc("RemoveShiftManager", Parameters1) != 0;
                    Parameters1.Clear();
                    Parameters1.Add("@hospital_id", inputHospital.hospital_id);
                    ViewBag.managers = dbm.ExecuteReader_proc("getShiftManagers", Parameters1);
                }
                else
                {
                    ViewBag.managers = managers;
                }
            }
            else
            {
                ViewBag.managers = managers;
            }

            return View();
        }



        public ActionResult RemoveBloodBag()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            ViewBag.BloodTypes = new List<object> { "All", "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            return View();
        }



        [HttpPost]
        public ActionResult RemoveBloodBag(BloodBag bag)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            ViewBag.BloodTypes = new List<object> { "All", "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@blood_type", bag.blood_type);
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            ViewBag.bloodbags = dbm.ExecuteReader_proc("getBloodBagsofType", Parameters1);

            return View();
        }



        public ActionResult RemoveBloodBagConfirmed(int blood_bag_id)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@blood_bag_id", blood_bag_id);



            if (dbm.ExecuteNonQuery_proc("RemoveBloodBag", Parameters1) != 0)
            {
                return RedirectToAction("RemoveBloodBag");
            }
            else
            {
                return Content("Fatal error");
            }
        }



        public ActionResult AddBloodBag()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            ViewBag.BloodTypes = new List<object> {"A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });

            return View();
        }

        [HttpPost]
        public ActionResult AddBloodBag(BloodBag bag, string date, string bloodPressure, string glucoseLevel)
        {

            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });

            Parameters1.Add("@national_id", Convert.ToInt64(bag.national_id));
            Parameters1.Add("@blood_bag_date", date);
            Parameters1.Add("@blood_camp_id", bag.blood_camp_id);
            Parameters1.Add("@notes", bag.notes);
            Parameters1.Add("@blood_type", bag.blood_type);

            
            ViewBag.added = (dbm.ExecuteNonQuery_proc("AddBloodBag", Parameters1) != 0); //This procedure adds the blood bag to stock and inserts the blood type in donor
            if(ViewBag.added == true)
            {
                Parameters1.Clear();
                Parameters1.Add("@hospital_id", inputHospital.hospital_id);
                Parameters1.Add("@national_id", Convert.ToInt64(bag.national_id));
                Parameters1.Add("@report_date", date);
                Parameters1.Add("@blood_pressure", bloodPressure);
                Parameters1.Add("@glucose_level", glucoseLevel);
                Parameters1.Add("@notes", bag.notes);

                dbm.ExecuteNonQuery_proc("insert_donorHealthInfo", Parameters1);
            }

            ViewBag.BloodTypes = new List<object> {"A+","B+","B-","O+","O-","AB+","AB-"};


            return View();

        }

        ////GET
        //public ActionResult consumeService()
        //{
        //    Hospital inputHospital = (Hospital)TempData["inputHospital"];

        //    if (inputHospital != null)
        //    {
        //        TempData["inputHospital"] = inputHospital;
        //    }
        //    else
        //    {
        //        return RedirectToAction("SignIn", "Hospitals");
        //    }
        //    return View();
        //}

        //POST
        [HttpPost]
        public ActionResult ConsumeService(string username, int service_id)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }
            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            DataTable ProvidedServices = dbm.ExecuteReader_proc("HospitalProvideServices", Parameters1);
            DataTable NotProvidedServices = dbm.ExecuteReader_proc("HospitalNotProvideServices", Parameters1);
            ViewBag.ProvidedServicesDT = ProvidedServices;
            if (NotProvidedServices != null)

                ViewBag.NotProvidedServices = NotProvidedServices.AsEnumerable().Select(row => new service
                {
                    service_id = Convert.ToInt32(row["service_id"]),
                    name = (row["name"]).ToString()
                });


            if (ProvidedServices != null)

                ViewBag.ProvidedServices = ProvidedServices.AsEnumerable().Select(row => new service
                {
                    service_id = Convert.ToInt32(row["service_id"]),
                    name = (row["name"]).ToString()
                });

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", username);

            DataTable pointsTable = dbm.ExecuteReader_proc("getUserPoints", Parameters);
            if(pointsTable == null)
            {
                return View("Services");
            }
            int userPoints = Convert.ToInt32(pointsTable.Rows[0]["points"]);

            Parameters.Clear();
            Parameters.Add("@hospital_id", inputHospital.hospital_id);
            Parameters.Add("@service_id", service_id);

            DataTable service = dbm.ExecuteReader_proc("getHospitalService", Parameters);
            if(service == null)
            {
                return View("Services");
            }
            int serviceValue = Convert.ToInt32(service.Rows[0]["value"]);

            if(userPoints < serviceValue)
            {
                ViewBag.lowPoints = true;
                return View("Services");
            }

            Parameters.Add("@username", username);
            Parameters.Add("@service_value", serviceValue);


            if (dbm.ExecuteNonQuery_proc("consumeService", Parameters) != 0)
            {
                ViewBag.successProcess = true;
                return View("Services");
            }
            else
            {
                return Content("Fatal Error");
            }
        }

        //GET
        public ActionResult SendNotification()
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }


            return View();
        }

        //POST
        [HttpPost]
        public ActionResult sendNotification(string bloodType, string info)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("SignIn", "Hospitals");
            }
            if(!String.IsNullOrEmpty(info))
            {
                Dictionary<string, object> Parameters = new Dictionary<string, object>();
                Parameters.Add("@blood_type", bloodType);
                DataTable usersOfBloodType = dbm.ExecuteReader_proc("getUsersOfBloodType", Parameters);
                if (usersOfBloodType != null)
                {
                    foreach (DataRow u in usersOfBloodType.Rows)
                    {
                        Parameters.Clear();
                        Parameters.Add("@username", u["username"]);
                        Parameters.Add("@info", info);
                        dbm.ExecuteNonQuery_proc("sendNotifications", Parameters);
                    }
                    ViewBag.success = true;
                }
            }
            else
            {
                ViewBag.success = false;

            }

            ViewBag.BloodTypes = new List<object> { "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            return View("Index", inputHospital);
        }

        [HttpPost]
        public ActionResult ChangePassword(string pass)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("Index", "Hospitals");
            }
            if (String.IsNullOrEmpty(pass))
            {
                return RedirectToAction("Index", "Hospitals");
            }
            if (pass.Length > 30)
            {
                return RedirectToAction("Index", "Hospitals");
            }
            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@username", inputHospital.username);
            Parameters.Add("@password", pass);
            if (dbm.ExecuteNonQuery_proc("changePassword", Parameters) != 0)
            {
                TempData["passSuccess"] = true;
            }
            return RedirectToAction("Index", "Hospitals");
        }

        [HttpPost]
        public ActionResult getVolunteerHealthInfo(string n_id)
        {
            Hospital inputHospital = (Hospital)TempData["inputHospital"];
            if (inputHospital != null)
            {
                TempData["inputHospital"] = inputHospital;
            }
            else
            {
                return RedirectToAction("Index", "Hospitals");
            }
            Int64 national_id = Convert.ToInt64(n_id);

            Dictionary<string, object> Parameters = new Dictionary<string, object>();
            Parameters.Add("@n_id", n_id);
            Parameters.Add("@h_id", inputHospital.hospital_id);

            DataTable volunteer = dbm.ExecuteReader_proc("getVolunteerHealthInfo", Parameters);
            ViewBag.BloodTypes = new List<object> { "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };
            if (volunteer == null)
            {
                return View("Index", inputHospital);
            }
            else
            {
                ViewBag.volunteer = volunteer;
                return View("Index", inputHospital);
            }
        }
        
    }
}




