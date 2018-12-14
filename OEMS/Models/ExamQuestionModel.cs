using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class ExamQuestionModel
    {
        public QuestionModel Question { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCorrect { get; set; }

    }
}