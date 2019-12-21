using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace BBMS.Models
{
    public class User
    {
        [Required]
        [Range(10000000000000, 99999999999999, ErrorMessage ="The national Id has to be 14 digits")]
        public Int64 national_id { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string name { get; set; }
        [Required]
        [RegularExpression("M|F", ErrorMessage = "Male or Female only")]
        public char gender { get; set; }
        [Required]
        [Range(18,65, ErrorMessage = "You can not register to this system if your age is out of the range(18 - 65)")]
        public int age { get; set; }
        [Required]
        [RegularExpression("01[0-9]{9}", ErrorMessage = "The phone number has to be in the format 01112223334")]
        public string phone { get; set; }
        [Required]
        [StringLength(30)] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string city { get; set; }
        [Required]
        [StringLength(30)] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]
        public string governorate { get; set; }
        public string blood_type { get; set; }
        public int points { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [RegularExpression("^[a-zA-Z0-9]+([_.-]?[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid username format")]
        public string username { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public int donation_count { get; set; }
    }
}
