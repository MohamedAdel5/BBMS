using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class HPS
    {
        public int hospital_id { get; set; }
        public int service_id_p { get; set; }
        public int value { get; set; }
        public string service_name { get; set; }
    }
}