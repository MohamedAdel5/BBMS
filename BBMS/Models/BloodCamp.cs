using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BBMS.Models
{
    public class BloodCamp
    {
        public int blood_camp_id { get; set; }
        public int hospital_id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string driver_name { get; set; }
    }
}