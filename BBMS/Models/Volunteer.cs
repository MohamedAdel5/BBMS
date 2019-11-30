using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class Volunteer
    {
        public ulong national_id { get; set; }
        public string name { get; set; }
        public char gender { get; set; }
        public int age { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string governorate { get; set; }
        public string blood_type { get; set; }
    }
}