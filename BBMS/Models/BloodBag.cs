using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class BloodBag
    {
        public ulong national_id { get; set; }
        public string date { get; set; }
        public int blood_camp_id { get; set; }
        public int hospital_id { get; set; }
        public string notes { get; set; }
    }
}