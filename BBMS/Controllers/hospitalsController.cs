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
            hospital inputHospital = (hospital)TempData["inputHospital"];
            TempData["inputHospital"] = inputHospital;
            return View(inputHospital);

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

        // POST: hospitals/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //GET: remove hospital View
        public ActionResult RemoveHospital()
        {
          
            /*Get all hospitals names and ids and send them to the view through the view bag*/
            return View();
        }

        //GET: remove hospital View
        [HttpPost]
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
            /*The viewBag is empty*/
            return View();
        }

        //POST: 
        /*
         * redirects the user to his profile pagee if the username and password are correct
         *else it throws an error and leaves you in the same page
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
                hospital inputHospital;
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
                    inputHospital = new hospital()
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
            hospital inputHospital = (hospital)TempData["inputHospital"];

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
             
            TempData["inputHospital"] = inputHospital;

            return View();

        }

        [HttpPost]

        public ActionResult AddService(Service srv)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@service_id", srv.service_id);
            Parameters1.Add("@value", srv.value);

            dbm.ExecuteNonQuery_proc("AddServiceToHospital", Parameters1);

            TempData["inputHospital"] = inputHospital;


            return Redirect("Services");
        }

        [HttpPost]
        public ActionResult RemoveService(Service srv)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@service_id", srv.service_id);

            dbm.ExecuteNonQuery_proc("RemoveServiceFromHospital", Parameters1);

            TempData["inputHospital"] = inputHospital;

            return Redirect("Services");
        }






        public ActionResult BloodCamps()
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

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


            TempData["inputHospital"] = inputHospital;

            return View();

        }

        public ActionResult AddCamp(BloodCamp camp)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            Parameters1.Add("@driver_name", camp.driver_name);

            dbm.ExecuteNonQuery_proc("AddBloodCamp", Parameters1);

            TempData["inputHospital"] = inputHospital;


            return Redirect("BloodCamps");
        }

        [HttpPost]
        public ActionResult RemoveCamp(BloodCamp camp)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_camp_id", camp.blood_camp_id);

            dbm.ExecuteNonQuery_proc("RemoveBloodCamp", Parameters1);

            TempData["inputHospital"] = inputHospital;


            return Redirect("BloodCamps");
        }




        public ActionResult RemoveShift()
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });


            TempData["inputHospital"] = inputHospital;

            return View();

        }



        [HttpPost]
        public ActionResult RemoveShift(Shift shift)
        {
            hospital inputHospital = (hospital)TempData["inputHospital"];



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


            TempData["inputHospital"] = inputHospital;




            return View();
        }



        public ActionResult RemoveShiftConfirmed(int blood_camp_id, string shift_date)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_camp_id", blood_camp_id);
            Parameters1.Add("@shift_date", shift_date);

            TempData["inputHospital"] = inputHospital;


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

            hospital inputHospital = (hospital)TempData["inputHospital"];



            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });


            TempData["inputHospital"] = inputHospital;




            return View();


        }

        [HttpPost]
        public ActionResult AddShift(Shift shift, string start, string end, string shift_date)
        {
            hospital inputHospital = (hospital)TempData["inputHospital"];

            start += ":00";
            end += ":00";

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_camp_id", shift.blood_camp_id);
            Parameters1.Add("@shift_date", shift_date);
            Parameters1.Add("@shift_manager_username", shift.shift_manager_username);
            Parameters1.Add("@start_hour", start);
            Parameters1.Add("@finish_hour", end);
            Parameters1.Add("@city", shift.city);
            Parameters1.Add("@governorate", shift.governorate);

            Parameters1.Clear();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });





            TempData["inputHospital"] = inputHospital;


            ViewBag.added = (dbm.ExecuteNonQuery_proc("insert_shift", Parameters1) != 0);
            
            return View();
            
            
        }




        public ActionResult AddShiftManager() {


            return View();
        }

        [HttpPost]
        public ActionResult AddShiftManager(ShiftManager mgr)
        {
            hospital inputHospital = (hospital)TempData["inputHospital"];
            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();



            Parameters1.Add("@username", mgr.username);
            Parameters1.Add("@user_pass", mgr.password);
            Parameters1.Add("@name", mgr.name);
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);


            TempData["inputHospital"] = inputHospital;


            ViewBag.added = (dbm.ExecuteNonQuery_proc("insert_shift_manager", Parameters1) != 0);

            return View();


        }


        public ActionResult RemoveShiftManager()
        {
            hospital inputHospital = (hospital)TempData["inputHospital"];

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            ViewBag.managers = dbm.ExecuteReader_proc("getShiftManagers", Parameters1);
            TempData["inputHospital"] = inputHospital;

            return View();
        }

        [HttpPost]
        public ActionResult RemoveShiftManager(ShiftManager mgr)
        {

            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            hospital inputHospital = (hospital)TempData["inputHospital"];

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            ViewBag.managers = dbm.ExecuteReader_proc("getShiftManagers", Parameters1);



           
            ViewBag.removed = ((DataTable)ViewBag.managers).AsEnumerable().Any(row => mgr.username == row.Field<String>("username"));

            if (ViewBag.removed == true)
            {
                Parameters1.Clear();
                Parameters1.Add("@username", mgr.username);
                ViewBag.removed = dbm.ExecuteNonQuery_proc("RemoveShiftManager", Parameters1) != 0;
            }

            TempData["inputHospital"] = inputHospital;


            return View();
        }



        public ActionResult RemoveBloodBag()
        {
            ViewBag.BloodTypes = new List<object> { "All", "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };

            return View();
        }



        [HttpPost]
        public ActionResult RemoveBloodBag(BloodBag bag)
        {
            hospital inputHospital = (hospital)TempData["inputHospital"];

            ViewBag.BloodTypes = new List<object> { "All", "A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };




            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_type", bag.blood_type);
            Parameters1.Add("@hospital_id", inputHospital.hospital_id);

            ViewBag.bloodbags = dbm.ExecuteReader_proc("getBloodBagsofType", Parameters1);



            TempData["inputHospital"] = inputHospital;




            return View();
        }



        public ActionResult RemoveBloodBagConfirmed(int blood_bag_id)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];


            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@blood_bag_id", blood_bag_id);

            TempData["inputHospital"] = inputHospital;


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
            ViewBag.BloodTypes = new List<object> {"A+", "B+", "B-", "O+", "O-", "AB+", "AB-" };


            hospital inputHospital = (hospital)TempData["inputHospital"];




            Dictionary<string, object> Parameters1 = new Dictionary<string, object>();

            Parameters1.Add("@hospital_id", inputHospital.hospital_id);
            DataTable BloodCamps = dbm.ExecuteReader_proc("GetBloodCamps", Parameters1);

            if (BloodCamps != null)

                ViewBag.BloodCamps = BloodCamps.AsEnumerable().Select(row => new BloodCamp
                {
                    blood_camp_id = Convert.ToInt32(row["blood_camp_id"]),
                    driver_name = (row["driver_name"]).ToString()
                });


            TempData["inputHospital"] = inputHospital;




            return View();
        }

        [HttpPost]
        public ActionResult AddBloodBag(BloodBag bag, string date)
        {

            hospital inputHospital = (hospital)TempData["inputHospital"];

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

            ViewBag.added = (dbm.ExecuteNonQuery_proc("AddBloodBag", Parameters1) != 0);


            ViewBag.BloodTypes = new List<object> {"A+","B+","B-","O+","O-","AB+","AB-"};


            TempData["inputHospital"] = inputHospital;
            return View();

        }


    }
}




