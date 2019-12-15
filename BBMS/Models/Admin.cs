using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace BBMS.Models
{
    public class Admin
    {
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