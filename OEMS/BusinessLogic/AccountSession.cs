using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OEMS
{
    public class AccountSession
    {
        public static UserModel User = new UserModel();
        public static List<CourseModel> AllCourses = new List<CourseModel>();

        #region Used by students 
        public static CourseModel Course = new CourseModel();
        public static List<ModuleModel> AllModules = new List<ModuleModel>();
        

        #endregion
        public AccountSession()
        {

        }
    }
}