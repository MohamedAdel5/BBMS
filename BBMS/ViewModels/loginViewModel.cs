using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BBMS.ViewModels
{
    public class loginViewModel
    {
        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [RegularExpression("^[a-zA-Z0-9]+([_.-]?[a-zA-Z0-9]+)*$", ErrorMessage = "Invalid username format")]
        [Required(ErrorMessage ="Please enter your username")]
        public string username { get; set; }

        [StringLength(30, ErrorMessage = "Max length = 30 characters")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please enter your password")]
        public string password { get; set; }
    }
}