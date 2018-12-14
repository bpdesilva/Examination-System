using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class RegisterController : Controller
    {
        private DBAccess DB = new DBAccess();

        [HttpGet]
        public ActionResult Lecturer()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Lecturer(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if ((new DBAccess().ValidateUsername(user.Username)))
                {
                    ViewBag.ErrorMessage = "Username already in use";
                    return View();
                }
                else
                {
                    var NewUser = new UserModel() {
                        Type = 'L',
                        Username = user.Username,
                        Password = user.Password,
                        FName = user.FName,
                        LName = user.LName
                    };


                    return RedirectToAction("Login");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Student()
        {
            var modules = DB.GetCourses();

            List<SelectListItem> AllCourses = new List<SelectListItem>();
            foreach (var item in modules)
            {
                
                var selection = new SelectListItem()
                {
                    Text = item.Name,
                    Value = item.CourseId.ToString()
                };
                AllCourses.Add(selection);
            }

            ViewBag.AllCourses = AllCourses;

            return View(new UserModel());
        }

        [HttpPost]
        public ActionResult Student(UserModel user)
        {
            user.Type = 'S';


            return View();
        }
    }
}