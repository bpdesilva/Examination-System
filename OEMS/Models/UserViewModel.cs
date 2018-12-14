using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "UserName is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        public string FName { get; set; }
        public string LName { get; set; }
        public string Area { get; set; }
        public string Course { get; set; }

        public Guid CourseItem { get; set; }
        public IEnumerable<SelectListItem> CourseList { get; set; }
        public int AccountId { get; set; }
    }
}