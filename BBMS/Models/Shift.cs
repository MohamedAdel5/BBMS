using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BBMS.Models
{
    public class Shift
    {
        [Required]
        public int blood_camp_id { get; set; }

        [Required]
        public string date { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [RegularExpression("^[a-zA-Z0-9]+([_.-]?[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid username format")]
        public string shift_manager_username { get; set; }

        [Required]
        public string start_hour { get; set; }
        [Required]
        public string finish_hour { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string city { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string governorate { get; set; }

    }
}