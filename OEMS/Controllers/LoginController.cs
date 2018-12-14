using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            if(AccountSession.User.AccountId != 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new UserViewModel());
        }

        [HttpPost]
        public ActionResult Index(UserViewModel model)
        {            
            if (new DBAccess().ValidateLogin(model.Username, model.Password))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.NotValidUser = "Invalid Credentials";

            }
            //else
            //{
            //    ViewBag.Failedcount = item;
            //}
            return View("Index");
        }
    }
}