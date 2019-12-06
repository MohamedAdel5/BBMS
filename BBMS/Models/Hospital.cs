using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BBMS.Models
{
    public class Hospital
    {
        public int hospital_id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string hospital_name { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string city { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string governorate { get; set; }


        [Required]
        [RegularExpression("01[0-9]{9}", ErrorMessage = "The phone number has to be in the format 01112223334")]
        public string phone { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [RegularExpression("^[a-zA-Z0-9]+([_.-]?[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid username format")]
        public string username { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}