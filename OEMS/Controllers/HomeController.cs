using OEMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class HomeController : Controller
    {
        UserModel Account = AccountSession.User;
        char AccType = AccountSession.User.Type;
        public ActionResult Index()
        {
            if(AccType.Equals('A')|| AccType.Equals('L') || AccType.Equals('S'))
            {
                return View(Account);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Login()
        {

            return View();
        }

        public ActionResult Signout()
        {
            AccountSession.User = new UserModel();
            return RedirectToAction("Index", "Login");
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}