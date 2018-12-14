using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class LecturerController : Controller
    {
        private DBAccess DB = new DBAccess();
        private static List<UserModel> Lecturers = new List<UserModel>();
        // GET: Lecturer
        public ActionResult Index()
        {
            Lecturers = DB.GetLecturers();
            return View(DB.GetLecturers());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                if ((DB.ValidateUsername(user.Username)))
                {
                    ViewBag.ErrorMessage = "Username already in use";
                    return View();
                }
                else
                {
                    var NewUser = new UserModel()
                    {
                        Type = 'L',
                        Username = user.Username,
                        Password = user.Password,
                        FName = user.FName,
                        LName = user.LName,
                        Area = user.Area
                    };

                    DB.CreateLecturer(NewUser);

                    return RedirectToAction("Index", "Lecturer");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var lecturers = DB.GetLecturers();
            var user = lecturers.Find(x => x.AccountId == id);
            var model = new UserViewModel()
            {
               Area = user.Area,
               FName = user.FName,
               LName = user.LName,
               Username = user.Username,
               Password = user.Password,
               AccountId = id
               
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newuser = new UserModel()
                {
                    Area = model.Area,
                    FName = model.FName,
                    LName = model.LName,
                    Name = model.FName +" "+ model.LName,
                    Username = model.Username,
                    Password = model.Password,
                    AccountId = model.AccountId
                };

                DB.UpdateLecturer(newuser);
            }

            return View("Index", DB.GetLecturers());
        }

        [HttpGet]
        public ActionResult View(int id)
        {
            var user = Lecturers.Find(x => x.AccountId == id);
            return View(user);
        }
    }
}