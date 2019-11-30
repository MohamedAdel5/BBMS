using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class Hospital
    {
        public int hospital_id { get; set; }
        public string name { get; set; }
        public string city { get; set; }
        public string governorate { get; set; }
        public string phone { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}