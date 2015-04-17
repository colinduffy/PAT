using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.Mvc;
using PAT_ELAC.Models;
using System.Web.Security;
using PAT_ELAC.Filters;
using WebMatrix.WebData;
using System.IO;

namespace PAT_ELAC.Controllers
{
    [InitializeSimpleMembership]
    public class HomeController : Controller
    {
        private TopicContext tDb = new TopicContext();
        private TestContext testDb = new TestContext();
        private TakenContext takenDb = new TakenContext();
        private TopicContext topDb = new TopicContext();
        private UsersContext userDb = new UsersContext();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            //if(User.IsInRole
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        public List<QM> getQuestionData()
        {
            var aDb = new AnswerContext().Answers.ToList();
            int size = aDb.Count();

            var aqDb = new AnsweredQuestionsContext().AnsweredQuestions.ToList();
            var data = new List<QM>();
            foreach (QuestionModel q in new QuestionContext().Questions.ToList())
            {
                var aQ = aqDb.Where(a => a.questionId == q.QuestionId);
                var answers = aDb.Where(a => a.QuestionId == q.QuestionId);

                int total = aQ.Count() <= 0 ? 1 : aQ.Count();
                int correct = 0;

                foreach (AnsweredQuestionsModel aqm in aQ)
                {
                    var tempAns = answers.Where(a => a.AnswerId == aqm.choice).First();

                    if (tempAns.IsCorrect)
                        correct++;
                }
                Topic topic = tDb.Topics.Where(t => t.TopicId == q.TopicId).First();
                if (topic == null){
                    topic = new Topic();
                    topic.description = " ";
                }
                data.Add(new QM(correct, total, q, topic));
            }
       /*     foreach (AnswerModel a in aDb)
            {
                int correct = 0;
                int total = 0;
                foreach (AnsweredQuestionsModel aq in aqDb)
                {
                    if (aq.choice == a.AnswerId)
                    {
                        total++;

                        if (a.IsCorrect)
                            correct++;
                    }
                }

                if (total > 0)
                {
                    data.Add(new QM(correct, total));

                }
            }*/
            return data;
        }

        public ActionResult Test(int subjectId = 0)
        {
            ViewBag.Message = "Testing Page: " + subjectId;

            //Todo Create Test base on subjectId

            return View();
        }



        [HttpPost]
        public ActionResult Test() //Todo add model
        {
            if (true)
            { //ModelState.isValid
                return RedirectToAction("Resouces"); //RedirectToAction("Action", new { id = 99 }); topics instead of id
            }

            return View();
        }


        public ActionResult ResourceRedirect(int t)
        {
            var taken = takenDb.TakenTests.Where(test => test.TakenId == t).First();
            return new TestController().Resources(taken);
        }

        public ActionResult StudentAccount()
        {
            ViewBag.Message = "Student Account Page.";

            var tests = testDb.Tests.ToList();
            var mathTests = new List<Test>();
            var englishTests = new List<Test>();
            foreach (Test t in tests)
            {
                if (t.SubjectId == 0 || t.SubjectId == 1)
                {
                    mathTests.Add(t);
                }
                if (t.SubjectId == 2)
                {
                    englishTests.Add(t);
                }
            }
            ViewData["MathTests"] = mathTests;
            //ViewData["EnglishTests"] = new SelectList(englishTests, "TestId", "Description");
            var model = new Student();
            model.MathTests = new SelectList(mathTests, "TestId", "Description");
            model.EnglishTests = new SelectList(englishTests, "TestId", "Description");

            model.EnglishPlacement = "";
            model.MathPlacement = "";

            
            var uId = WebSecurity.GetUserId(User.Identity.Name);
            var taken = takenDb.TakenTests.Where(t => t.UserId == uId).ToList();

            model.Tests = new List<TakenReview>();

            var tIds = new HashSet<int>();
            model.MathPlacement = getPlacement(0, uId);//"N/A";
            model.EnglishPlacement = getPlacement(2, uId);
            foreach(TakenTest t in taken)
            {
                var tr = new TakenReview();
                tr.Test = t;
               // var tempTest = new Test();

                foreach (Test test in testDb.Tests)
                {
                    if (t.TestId == test.TestId)
                    {
                        //tID = t.TestId;
                        tr.TestDescription = test.Description;
                        model.Tests.Add(tr);

                       /* if (t.Passed)
                        {
                            if (tIds.Contains(t.TestId))
                                continue;

                            tIds.Add(t.TestId);

                            if (test.SubjectId == 0 || test.SubjectId == 1)
                                model.MathPlacement += test.Description + ", ";
                            else
                                model.EnglishPlacement += test.Description + ", ";
                        }*/

                    }
                }
            }



            return View(model);
        }

        [HttpPost]
        public ActionResult StudentAccount(Student model, string Command)
        {
            if (Command == "math")
            {
                return TakeTest(model.MathId);
            }
            else if (Command == "english")
            {
                return TakeTest(model.EnglishId);            
            }

            return View();
        }

        public ActionResult TakeTest(int testId)
        {
            var q = new QuestionContext().Questions.ToList();//.OrderBy(QuestionId => new Random().Next());
            var a = new AnswerContext().Answers.ToList();//.OrderBy(AnswerId => new Random().Next());

            Random rand = new Random();
            int rnd = rand.Next();

            

            if (rnd % 2 == 0)
            {
                q.Reverse();
                a.Reverse();
            }


            var tests = new TestContext().Tests.ToList();

            var threshold = 0;
            foreach (Test t in tests)
            {
                if (t.TestId == testId)
                {
                    threshold = t.Threshold;
                    break;
                }
            }


            var topics = new TestTopicsContext().TestTopics.ToList();
            var questions = new List<QuestionModel>();
            var testquestions = new List<TestQuestion>();

            var testValue = 0;
            foreach (TestTopics t in topics)
            {
                if (t.TestId == testId)
                {
                    //TestQuestion tq = new TestQuestion();

                    int num = 0;
                    foreach (QuestionModel ques in q)
                    {
                        TestQuestion tq = new TestQuestion();
                         var Answers = new List<AnswerModel>();
                        
                        if (ques.TopicId == t.TopicId)
                        {
                            testValue += t.QuestionValue;
                            tq.Value = t.QuestionValue;
                            questions.Add(ques);
                            tq.Question = ques;
                            //tq.SelectedAnswerId = -1;
                            foreach (AnswerModel ans in a)
                            {
                                if(ans.QuestionId == ques.QuestionId)
                                    Answers.Add(ans);
                            }
                            tq.Answers = Answers.ToArray();
                            
                            testquestions.Add(tq);
                            num++;
                            if (num == t.Quantity) break;
                        }
                    }
            
              
                    //questions.Concat(q.Where(qu => qu.TopicId == t.TopicId).Take(t.Quantity));
                }
            }
            //return new TestController().TakeTest(testquestions);
            TempData["TESTQUESTIONS"] = testquestions;
            TempData["TESTID"] = testId;
            TempData["TESTVAL"] = testValue;
            TempData["THRESHOLD"] = threshold;
            return RedirectToAction("TakeTest", "Test");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminAccount()
        {
            ViewBag.Message = "Admin Account Page.";
            return View(getQuestionData());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminStudentInfo()
        {

            var model = new List<AdminStudentInfo>();

            foreach (UserProfile user in userDb.UserProfiles.ToList())
            {
                var temp = new AdminStudentInfo();
                temp.LastName = user.LastName;
                temp.FirstName = user.FirstName;
                temp.StudentID = user.StudentID;
                temp.Phone = user.Phone;
                temp.MathPlacement = getPlacement(0, user.UserId);//"N/A";
                temp.EnglishPlacement = getPlacement(2, user.UserId); //"N/A";
                model.Add(temp);
            }

            ViewBag.Message = "Admin Student Info Page.";
            return View(model);
        }

        private string getPlacement(int sId, int uId)
        {
            String placement = "N/A";
            var taken = takenDb.TakenTests.Where(t => t.UserId == uId).ToList();
            int level = -1;
            foreach (TakenTest t in taken)
            {
                var tr = new TakenReview();
                tr.Test = t;
                var tempTest = new Test();
                foreach (Test test in testDb.Tests)
                {
                    if (t.TestId == test.TestId && (test.SubjectId == sId || (sId == 0 && test.SubjectId ==1)))
                    {
                        if (t.Passed && test.Level > level)
                        {
                            placement = test.Description;
                        }
                    }
                }

            }

            return placement;
        }


        public ActionResult Resources()
        {
            ViewBag.Message = "The Resources Page.";
            return View();
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {

            if (file.ContentLength > 0 && file.FileName.EndsWith(".csv"))
            {
                string result = new StreamReader(file.InputStream).ReadToEnd();
                string[] lines = result.Split('\n');
                foreach (string s in lines)
                { 
                    string[] line = s.Split(',');
                    if(line.Length < 3)
                        continue;

                    string topic = line[0];
                    string question = line[1];

                    int topicId = -1;

                    var topics = tDb.Topics.Where(t => t.description.ToUpper() == topic.ToUpper());
                    if (topics.Count() == 0)
                    {
                        //create new topic
                        var newT = new Topic();
                        newT.description = topic;
                        newT.Message = topic + " message";
                        tDb.Topics.Add(newT);
                        tDb.SaveChanges();
                        topicId = newT.TopicId;
                    }
                    else 
                    {
                        topicId = topics.First().TopicId;
                    }

                    QuestionModel q = new QuestionModel();
                    q.Question = question;
                    q.TopicId = topicId;
                    var qDb = new QuestionContext();
                    qDb.Questions.Add(q);
                    qDb.SaveChanges();

                    var aDb = new AnswerContext();

                    for (int i = 2; i < line.Length; i++)
                    {
                        AnswerModel answer = new AnswerModel();
                        answer.QuestionId = q.QuestionId;
                        answer.Answer = line[i];
                        answer.IsCorrect = i == 2 ? true : false;
                        aDb.Answers.Add(answer);
                    }
                    aDb.SaveChanges();

                }

            }
            return RedirectToAction("AdminAccount");
        }



        [HttpParamAction]
        public ActionResult Save(Object model)
        {
            // ...
            return RedirectToAction("Test", "Home");
        }
    }
}
