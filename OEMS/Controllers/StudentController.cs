using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class StudentController : Controller
    {
        private DBAccess DB = new DBAccess();
        private static List<UserModel> Students = new List<UserModel>();
        // GET: Student
        public ActionResult Index()
        {
            Students = DB.GetStudents();
            return View(Students);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var userviewmodel = new StudentViewModel();
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

            userviewmodel.CourseList = AllCourses;
            //userviewmodel.Username =  DB.GetNextId().ToString();
            return View(userviewmodel);
        }

        [HttpPost]
        public ActionResult Create(StudentViewModel user)
        {
            if (ModelState.IsValid)
            {

                var NewUser = new UserModel()
                {
                    Type = 'S',
                    Username = user.FName,
                    Password = user.Password,
                    FName = user.FName,
                    LName = user.LName,
                    CourseId = user.CourseId
                };

                DB.CreateStudent(NewUser);

                return RedirectToAction("Index", "Student");

            }

            // If the ModelState is not valid, the model rebuilding to return
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

            user.CourseList = AllCourses;

            return View();
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            var student = DB.GetStudent(id);
            var model = new StudentViewModel()
            {
                Username = student.Username,
                CourseId = student.CourseId,
                FName = student.FName,
                LName = student.LName
            };
            model.Course = DB.GetCourse(Int32.Parse(model.CourseId)).Name;
            model.AvgMarks = DB.GetAvgMarks(student.StudentId);

            return View(model);
        }

        [HttpGet]
        public ActionResult AllView()
        {
            List<StudentViewModel> viewModels = new List<StudentViewModel>();
            var students = DB.GetStudents();
            foreach(UserModel student in students)
            {
                viewModels.Add(new StudentViewModel()
                {
                    Course = DB.GetCourse(Int32.Parse(student.CourseId)).Name,
                    FName = student.FName,
                    LName = student.LName,
                    AvgMarks = DB.GetAvgMarks(student.StudentId)
                });
            }

            return View(viewModels);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var student = DB.GetStudent(id);
            var model = new UserViewModel()
            {
                Username = student.Username,
                FName = student.FName,
                LName = student.LName,
                Password = student.Password,
                AccountId = student.AccountId
            };
            model.Course = DB.GetCourse(Int32.Parse(student.CourseId)).Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            var student = new UserModel()
            {
                Username = model.Username,
                FName = model.FName,
                LName = model.LName,
                Password = model.Password,
                AccountId = model.AccountId,
                Name = model.FName + " " + model.LName,

            };

            DB.UpdateStudent(student);

            return View("Index", DB.GetStudents());
        }
    }
}