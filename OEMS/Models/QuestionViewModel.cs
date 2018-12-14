using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class QuestionViewModel
    {
        public int QuestionID { get; set; }
        public int ModuleID { get; set; }

        [Required(ErrorMessage = "Question is required")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Correct Answer is required")]
        public string CorrectAns { get; set; }

        [Required(ErrorMessage = "Alternate Answer is required")]
        public string AlternateAnsOne { get; set; }

        [Required(ErrorMessage = "Alternate Answer is required")]
        public string AlternateAnsTwo { get; set; }

        [Required(ErrorMessage = "Alternate Answer is required")]
        public string AlternateAnsThree { get; set; }

    }
}