using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class CourseController : Controller
    {
        private DBAccess DB = new DBAccess();

        // GET: Course
        public ActionResult Index()
        {
            var Courses = DB.GetCourses();
            return View(Courses);
        }

        //Create Course View
        [HttpGet]
        public ActionResult Create()
        {
            return View(new CourseViewModel());
        }

        //Create Course
        [HttpPost]
        public ActionResult Create(CourseViewModel model)
        {
            CourseModel course = new CourseModel()
            {
                Name = model.Name
            };
            DB.CreateCourse(course);

            return RedirectToAction("Index");
        }
        //Edit courses
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var course = DB.GetCourse(id);
            var model = new CourseViewModel()
            {
                CourseId = course.CourseId,
                Name = course.Name,
                ModulesCount = course.ModulesCount,
                Modules = course.Modules
            };

            var modules = DB.GetModules();
            model.AllModules = modules;

            List<SelectListItem> AllModules = new List<SelectListItem>();
            foreach (var item in modules)
            {
                if (!(model.Modules.Any(x => x.ModuleID == item.ModuleID)))
                {
                    var selection = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = model.CourseId + "," + item.ModuleID.ToString()
                    };
                    AllModules.Add(selection);
                }
            }

            ViewBag.AllModules = AllModules;

            return View(model);
        }
        // Edit page view
        [HttpPost]
        public ActionResult Edit(CourseViewModel model)
        {
            return View();
        }
        //Add Modules to course
        [HttpPost]
        public ActionResult AddModule(string AllModules)
        {
            string[] ids = AllModules.Split(',');

            DB.AddCourseModules(Int32.Parse(ids[0]), Int32.Parse(ids[1]));

            return RedirectToAction("Edit/" + ids[0], "Course");
        }
        //Remove Module
        public ActionResult RemoveModule(int cid, int mid)
        {

            DB.RemoveCourseModules(cid,mid);

            return RedirectToAction("Edit/" + cid, "Course");
        }

    }
}