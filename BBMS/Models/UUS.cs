using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class UUS
    {

        public int user_service_id { get; set; }
        public Nullable<long> national_id { get; set; }
        public int hospital_id { get; set; }
        public int service_id_s { get; set; }
        public string service_use_date { get; set; }
    }
}