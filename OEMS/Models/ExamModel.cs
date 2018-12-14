using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class ExamModel
    {
        public int ExamId { get; set; }
        public int StudentId { get; set; }
        public int ModuleId { get; set; }
        public int Attempts { get; set; }
        public int Marks { get; set; }
    }
}