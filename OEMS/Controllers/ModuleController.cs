using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using OEMS.Models;

namespace OEMS.Content
{
    public class ModuleController : Controller
    {
        private DBAccess DB = new DBAccess();
        private static List<ModuleModel> Modules = new List<ModuleModel>(); 
        // GET: Module
        public ActionResult Index()
        {
            Modules = DB.GetModules();
            return View(Modules);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new ModuleViewModel());
        }

        [HttpPost]
        public ActionResult Create(ModuleViewModel ViewModule)
        {
            ModuleModel module = new ModuleModel()
            {
                Name = ViewModule.Name
            };
            DB.CreateModule(module);

            return RedirectToAction("Index");
        }

        
        [HttpGet]
        public ActionResult Edit(int Id)
        {
            ModuleModel Module = DB.GetModule(Id);
            ModuleViewModel ViewModel = new ModuleViewModel()
            {
                ModuleID = Module.ModuleID,
                Name = Module.Name,
                Questions = Module.Questions
            };
            return View(ViewModel);
        }

        [HttpPost]
        public ActionResult Edit(ModuleViewModel ViewModel)
        {
            return View();
        }

        #region Question
        [HttpGet]
        public ActionResult CreateQuestion(int id)
        {
            var questionModel = new QuestionViewModel()
            {
                ModuleID = id
            };
            return View(questionModel);
        }

        [HttpPost]
        public ActionResult CreateQuestion(QuestionViewModel ViewModule)
        {
            QuestionModel question = new QuestionModel()
            {
                ModuleID = ViewModule.ModuleID,
                Question = ViewModule.Question,
                Answer = ViewModule.CorrectAns,
                AlternateAnsOne = ViewModule.AlternateAnsOne,
                AlternateAnsTwo = ViewModule.AlternateAnsTwo,
                AlternateAnsThree = ViewModule.AlternateAnsThree
            };
            DB.CreateQuestion(question);

            return RedirectToAction("Edit/"+ViewModule.ModuleID, "Module");
        }

        [HttpGet]
        public ActionResult EditQuestion(int id)
        {
            QuestionModel question = DB.GetQuestion(id);
            QuestionViewModel model = new QuestionViewModel()
            {
                ModuleID = question.ModuleID,
                QuestionID = question.QuestionID,
                Question = question.Question,
                CorrectAns = question.Answer,
                AlternateAnsOne = question.AlternateAnsOne,
                AlternateAnsTwo = question.AlternateAnsTwo,
                AlternateAnsThree = question.AlternateAnsThree
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult EditQuestion(QuestionViewModel ViewModule)
        {
            QuestionModel question = new QuestionModel()
            {
                QuestionID = ViewModule.QuestionID,
                ModuleID = ViewModule.ModuleID,
                Question = ViewModule.Question,
                Answer = ViewModule.CorrectAns,
                AlternateAnsOne = ViewModule.AlternateAnsOne,
                AlternateAnsTwo = ViewModule.AlternateAnsTwo,
                AlternateAnsThree = ViewModule.AlternateAnsThree
            };

            DB.EditQuestion(question);
            return RedirectToAction("Edit/" + ViewModule.ModuleID, "Module");

        }


        public ActionResult DeleteQuestion(int qid,int mid)
        {
            DB.DeleteQuestion(qid);
            return RedirectToAction("Edit/" + mid, "Module");
        }
        #endregion
    }
}