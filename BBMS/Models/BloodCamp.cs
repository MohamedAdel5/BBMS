using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class BloodCamp
    {
        public int blood_camp_id { get; set; }
        public int hospital_id { get; set; }
        public string driver_name { get; set; }
    }
}