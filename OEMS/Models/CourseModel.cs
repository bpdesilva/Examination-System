using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class CourseModel
    {
        public int CourseId { get; set; }
        public string Name { get; set; }
        public int ModulesCount { get; set; }
        public List<ModuleModel> Modules { get; set; }

        public CourseModel()
        {
            Modules = new List<ModuleModel>();
        }
    }
}