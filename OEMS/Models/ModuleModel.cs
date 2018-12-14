using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class ModuleModel
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public int LecturerID { get; set; }
        public string Lecturer { get; set; }
        public List<QuestionModel> Questions { get; set; }

        public ModuleModel()
        {
            Questions = new List<QuestionModel>();
        }
    }
}