using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BBMS.Models
{
    public class Shift
    {
        public int blood_camp_id { get; set; }
        public string date { get; set; }
        public string shift_manager_username { get; set; }
        public short start_hour { get; set; }
        public short finish_hour { get; set; }
        public string city { get; set; }
        public string governorate { get; set; }

    }
}