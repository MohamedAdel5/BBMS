using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBMS.Models
{
    public class Hospital_Provide_Service
    {
        public int hospital_id { get; set; }


        [Required]
        [StringLength(30, ErrorMessage = "Max length = 30 characters")] /*maximum length = 30*/
        [RegularExpression("^[a-zA-Z]+(([',. -][a-zA-Z ])?[a-zA-Z]*)*$", ErrorMessage = "Only letters are allowed")]


        [Key]
        public string service_name { get; set; }
        public int  value { get; set; }
        
    }
}

