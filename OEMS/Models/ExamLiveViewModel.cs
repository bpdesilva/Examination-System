using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class ExamLiveViewModel
    {
        public int StudentId { get; set; }
        public List<ExamQuestionModel> ExamQuestions { get; set; }
        public int CurrentQuestionId { get; set; }
        public string Question { get; set; }
        public int Attempt { get; set; }
        public string[] Answers { get; set; }
        public string Answer { get; set; }
        public int TotalMarks { get; set; }
        public string LiveModelJson { get; set; }
        public int ModuleId { get; set; }
    }
}