using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAT_ELAC.Models;
using WebMatrix.WebData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using PAT_ELAC.Filters;
using PAT_ELAC.Models;

namespace PAT_ELAC.Controllers
{
    [InitializeSimpleMembership]
    public class TestController : Controller
    {
        private TestContext db = new TestContext();
        private TestTopicsContext dbTestTopics = new TestTopicsContext();
        private TakenContext tdbo = new TakenContext();
        private ResourceContext rdbo = new ResourceContext();
        private TopicContext topicdbo = new TopicContext();
        private TestTopicsContext testTopicsDB = new TestTopicsContext();
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View(db.Tests.ToList());
        }

  

        public ActionResult Resources(TakenTest taken)
        {
            var model = new ResourceModel();
            model.TestResult = tdbo.TakenTests.Where(test => test.TakenId == taken.TakenId).First();
            Test t = db.Tests.Where(test => test.TestId == taken.TestId).First();
            model.Topics = new List<Topic>();

            var testTopics = testTopicsDB.TestTopics.Where(topic => topic.TestId == taken.TestId);
            var topicIds = new HashSet<int>();
            
            foreach (TestTopics tt in testTopics)
            {
                if (tt.TestId == taken.TestId)
                {
                    model.Topics.Add(topicdbo.Topics.Where(topic => topic.TopicId == tt.TopicId).First());
                    topicIds.Add(tt.TopicId);
                }
            }

            model.Videos = new List<String>();
            foreach (Resource r in rdbo.Resources)
            { 
                if(topicIds.Contains(r.TopicId))
                    model.Videos.Add(r.Link);
            }

            return View("../Test/Resources", model);
        }


        public ActionResult TakeTest()//IEnumerable<TestQuestion> model)
        {
            var questions = TempData["TESTQUESTIONS"] as IEnumerable<TestQuestion>;
            var testId = TempData["TESTID"];

            var answers = new int[questions.Count()];
            for (int i = 0; i < answers.Count(); i++) { answers[i] = -1; }
            
            var model = new TakeTest();
            model.Questions = questions.ToArray();
            model.Answers = answers;
            model.TestId = Convert.ToInt32(testId);
            model.Threshold = Convert.ToInt32(TempData["THRESHOLD"]);
            
            return View(model);
        }

        [HttpPost]
        public ActionResult TakeTest(TakeTest test)
        {
            //var tdbo = new TakenContext();
            var newTest = new TakenTest();
            newTest.TestId = test.TestId;
            newTest.UserId = WebSecurity.GetUserId(User.Identity.Name);

            var questions = TempData["TESTQUESTIONS"] as IEnumerable<TestQuestion>;
            var testId = TempData["TESTID"];

            var aqdbo = new AnsweredQuestionsContext();

            float score = 0;
            float total = 0;

            var answerList = new AnswerContext().Answers.ToList();

            var tempAnswers = new List<AnsweredQuestionsModel>();

            //Grade the test
            for (int i = 0; i < test.Questions.Count(); i++)
            {
                var next = new AnsweredQuestionsModel();
                next.questionId = test.Questions[i].Question.QuestionId;
                next.choice = test.Answers[i];

                foreach (AnswerModel a in answerList)
                {
                    if (a.AnswerId == test.Answers[i])
                    {
                        if (a.IsCorrect)
                            score += test.Questions[i].Value;

                        total += test.Questions[i].Value;
                        break;
                    }
                }

               // next.takenId = newTest.TakenId;
                ///aqdbo.AnsweredQuestions.Add(next);
                tempAnswers.Add(next);
            }
            newTest.Score = Convert.ToInt32(score);
            var percent = score / total;
            newTest.Passed = ((percent*100) >= test.Threshold) ? true : false;

            tdbo.TakenTests.Add(newTest);
            tdbo.SaveChanges();

            int id = tdbo.TakenTests.Max(item => item.TakenId);
            newTest.TakenId = id;
            foreach (AnsweredQuestionsModel aqm in tempAnswers)
            {
                aqm.takenId = id;
                aqdbo.AnsweredQuestions.Add(aqm);
            }

            aqdbo.SaveChanges();


           // ViewData["TakenId"] = 1;

            return Resources(newTest);//RedirectToAction("Index", "Home");
        }
        
        //
        // GET: /Test/Details/5

        public ActionResult Details(int id = 0)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        //
        // GET: /Test/Create

        public ActionResult Create()
        {
            var subjects = new SubjectContext().Subjects.ToList();
            var model = new Test();

            model.TestTopic = new TestTopics[5];
            for (int i = 0; i < 5; i++)
            {
                model.TestTopic[i] = new TestTopics();
                model.TestTopic[i].Quantity = 0;
                model.TestTopic[i].QuestionValue = 1;
            }

            model.Subjects = new SelectList(subjects, "SubjectId", "Description");

            var topics = new TopicContext().Topics.ToList();
            model.Topics = new SelectList(topics, "TopicId", "description");
            
        
            return View(model);
        }

        //
        // POST: /Test/Create

        [HttpPost]
        public ActionResult Create(Test test)
        {
            if (ModelState.IsValid)
            {
                db.Tests.Add(test);
                db.SaveChanges();

                var tests = db.Tests.ToList();
                var testId = -1;
                foreach (Test t in tests)
                {
                    if (t.TestId > testId)
                        testId = t.TestId;
                }

                if (testId == -1)
                { 
                    //TODO throw error
                }

                String update = "Need to add\n";

                foreach (TestTopics t in test.TestTopic)
                {
                    if (t.Quantity > 0)
                    {
                        var q = new QuestionContext().Questions.Where(que => que.TopicId == t.TopicId).Count();
                        if(t.Quantity > q){
                            var top = new TopicContext().Topics.Where(to => to.TopicId == t.TopicId).First();
                            update += q - t.Quantity + " " + top.description + " questions\n";
                        }
                        t.TestId = testId;
                        dbTestTopics.TestTopics.Add(t);
                    }
                }

                dbTestTopics.SaveChanges();

                if (update == "Need to add\n") return RedirectToAction("Index");

                throw new Exception(update);
            }

            return View(test);
        }

        //
        // GET: /Test/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        //
        // POST: /Test/Edit/5

        [HttpPost]
        public ActionResult Edit(Test test)
        {
            if (ModelState.IsValid)
            {
                db.Entry(test).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(test);
        }

        //
        // GET: /Test/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Test test = db.Tests.Find(id);
            if (test == null)
            {
                return HttpNotFound();
            }
            return View(test);
        }

        //
        // POST: /Test/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Test test = db.Tests.Find(id);
            db.Tests.Remove(test);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            dbTestTopics.Dispose();
            base.Dispose(disposing);
        }
    }
}