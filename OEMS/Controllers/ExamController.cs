using OEMS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OEMS.Controllers
{
    public class ExamController : Controller
    {
        private DBAccess DB = new DBAccess();
        private static List<ExamModel> Exams = new List<ExamModel>();

        // GET: Exam
        public ActionResult Index()
        {
            Exams = DB.GetExams(AccountSession.User.StudentId);

            List<ModuleModel> coursemodules = DB.GetCourseModules(AccountSession.Course.CourseId);
            List<SelectListItem> ListModules = new List<SelectListItem>();

            foreach (var item in coursemodules)
            {
                if (!(Exams.Any(x => x.ModuleId == item.ModuleID)))
                {
                    var selection = new SelectListItem()
                    {
                        Text = item.Name,
                        Value = AccountSession.User.StudentId + "," + item.ModuleID
                    };
                    ListModules.Add(selection);
                }
            }

            ViewBag.ListModules = ListModules;

            List<ExamViewModel> ViewModel = new List<ExamViewModel>();
            foreach(var item in Exams)
            {
                ViewModel.Add(new ExamViewModel()
                {
                    StudentId = item.StudentId,
                    ModuleId = item.ModuleId,
                    ExamId = item.ExamId,
                    Attempts = item.Attempts,
                    Marks = item.Marks,
                    ModuleName = (DB.GetModule(item.ModuleId)).Name
                });
            }

            return View(ViewModel);
        }

        public ActionResult Create(string ListModules)
        {
            string[] ids = ListModules.Split(',');

            DB.AddExam(Int32.Parse(ids[0]), Int32.Parse(ids[1]));

            return RedirectToAction("Index", "Exam");
        }

        [HttpGet]
        public ActionResult Exam(int mid, int sid)
        {
            var exam = DB.GetExam(sid, mid);
            if (exam.Attempts < 3 || exam.Marks < 50)
            {
                List<QuestionModel> questions = DB.GetRanQuestions(mid);
                if (questions.Count()<15)
                {
                    return View("LowQuestions");
                }
                List<ExamQuestionModel> examQuestions = new List<ExamQuestionModel>();
                foreach (var item in questions)
                {
                    examQuestions.Add(new ExamQuestionModel()
                    {
                        Question = item,
                        IsCompleted = false,
                        IsCorrect = false
                    });
                }

                ExamLiveViewModel LiveModel = new ExamLiveViewModel()
                {
                    StudentId = sid,
                    ModuleId = mid,
                    ExamQuestions = examQuestions
                };
                LiveModel.Question = LiveModel.ExamQuestions[LiveModel.CurrentQuestionId].Question.Question;
                string[] Answers = {
                LiveModel.ExamQuestions[LiveModel.CurrentQuestionId].Question.Answer,
                LiveModel.ExamQuestions[LiveModel.CurrentQuestionId].Question.AlternateAnsOne,
                LiveModel.ExamQuestions[LiveModel.CurrentQuestionId].Question.AlternateAnsTwo,
                LiveModel.ExamQuestions[LiveModel.CurrentQuestionId].Question.AlternateAnsThree
            };
                Random rnd = new Random();
                Answers = Answers.OrderBy(x => rnd.Next()).ToArray();
                LiveModel.Answers = Answers;

                LiveModel.LiveModelJson = JsonConvert.SerializeObject(LiveModel);
                return View(LiveModel);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult Exam(ExamLiveViewModel Exam)
        {
            ModelState.Clear();
            ExamLiveViewModel ViewModel = JsonConvert.DeserializeObject<ExamLiveViewModel>(Exam.LiveModelJson);
            ViewModel.Answer = Exam.Answer;
            ViewModel.StudentId = Exam.StudentId;
            ViewModel.ModuleId = Exam.ModuleId;
            ViewModel.CurrentQuestionId = Exam.CurrentQuestionId;
            ViewModel.TotalMarks = Exam.TotalMarks;
            if (ViewModel.Answer.Equals(ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.Answer))
            {
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].IsCorrect = true;
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].IsCompleted = true;
                ViewModel.TotalMarks = ViewModel.TotalMarks + 1;
            }
            else
            {
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].IsCorrect = false;
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].IsCompleted = true;
            }

            ViewModel.CurrentQuestionId++;
            if (ViewModel.CurrentQuestionId < ViewModel.ExamQuestions.Count())
            {
                ViewModel.Question = ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.Question;
                string[] Answers = {
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.Answer,
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.AlternateAnsOne,
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.AlternateAnsTwo,
                ViewModel.ExamQuestions[ViewModel.CurrentQuestionId].Question.AlternateAnsThree
                };
                Random rnd = new Random();
                Answers = Answers.OrderBy(x => rnd.Next()).ToArray();
                ViewModel.Answers = Answers;


                ViewModel.LiveModelJson = JsonConvert.SerializeObject(ViewModel);
                return View("Exam",ViewModel);
            }
            else
            {
                ViewModel.TotalMarks = (int)(((ViewModel.TotalMarks*1.0)/ViewModel.ExamQuestions.Count()) * 100);
                var exam = DB.GetExam(ViewModel.StudentId, ViewModel.ModuleId);

                exam.Attempts++;
                exam.Marks = ViewModel.TotalMarks;
                DB.UpdateExam(exam);

                ViewModel.Attempt = exam.Attempts;
                return View("Results",ViewModel);
            }

        }
    }
}