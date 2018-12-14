using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class CourseViewModel
    {
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Name is required")]//error message
        public string Name { get; set; }
        public int ModulesCount { get; set; }
        public List<ModuleModel> Modules { get; set; }
        public IEnumerable<ModuleModel> AllModules { get; set; }
        public int SelectedModule { get; set; }

        public CourseViewModel()
        {
            Modules = new List<ModuleModel>();
            AllModules = new List<ModuleModel>();
        }
    }
}