using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OEMS.Models
{
    public class UserModel
    {
        public int AccountId { get; set; }
        public int LecturerId { get; set; }
        public int StudentId { get; set; }
        public char Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Area { get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }

    }
}