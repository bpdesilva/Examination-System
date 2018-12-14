using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class ModuleViewModel
    {
        public int ModuleID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public ModuleViewModel()
        {
            Questions = new List<QuestionModel>();
        }


    }
}