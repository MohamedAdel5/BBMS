using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BBMS.Models
{
    public class BloodBag
    {
        [Required]
        [Range(10000000000000, 99999999999999, ErrorMessage = "The national Id has to be 14 digits")]
        public ulong national_id { get; set; }


        [Required]
        public string date { get; set; }
        public int blood_camp_id { get; set; }

        public int hospital_id { get; set; }
        [Required]
        public string blood_type { get; set; }
        [StringLength(100, ErrorMessage = "Max length = 100 characters")] /*maximum length = 100*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string notes { get; set; }
    }
}