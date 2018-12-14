using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Models
{
    public class StudentViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]//error message 
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "First Name is required")]//error message 
        public string FName { get; set; }
        public string LName { get; set; }

        [Required(ErrorMessage = "Course is required")]//error message 
        public string CourseId { get; set; }
        public IEnumerable<SelectListItem> CourseList { get; set; }

        public string Course { get; set; }
        public int AvgMarks { get; set; }
    }
}