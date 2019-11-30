using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class DonorHealthInfo
    {
        public int national_id { get; set; }
        public int hospital_id { get; set; }
        public string date { get; set; }
        public string blood_pressure { get; set; }
        public string glucose_level { get; set; }
        public string notes { get; set; }
    }
}