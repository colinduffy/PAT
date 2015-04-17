using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAT_ELAC.Models;


namespace PAT_ELAC.Controllers
{
    public class QuestionController : Controller
    {
        private TopicContext tdb = new TopicContext();
        private QuestionContext qdb = new QuestionContext();
        private AnswerContext adb = new AnswerContext();

        //
        // GET: /Question/

        public ActionResult Index()
        {
            return View(qdb.Questions.ToList());
        }

        //
        // GET: /Question/Details/$id
        public ActionResult Details(int id = 0)
        {
            QuestionModel question = qdb.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // GET: /Question/Create

        public ActionResult Create()
        {

            var topics = new TopicContext().Topics.ToList();
            var temp = new List<TTemp>();
            foreach (Topic t in topics)
            { 
                var tmp = new TTemp();
                tmp.description = t.description;
                tmp.TopicId = t.TopicId;
                temp.Add(tmp);
            }
            var model = new QuestionCreateModel();
            model.Answers = new AnswerModel[5];
            for (int i = 0; i < 5; i++)
            {
                model.Answers[i] = new AnswerModel();
                model.Answers[i].IsCorrect = false;
            }

            model.Topics = new SelectList(temp, "TopicId", "description");
            //model.TopicID = -1;
            return View(model);
        }

        class TTemp
        {
            public int TopicId { get; set; }
            public String description { get; set; }
        }

        //
        // POST: /Question/Create

        [HttpPost]
        public ActionResult Create(QuestionCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var adb = new AnswerContext();
                
                var q = new QuestionModel();
                q.Question = model.Question;
                q.TopicId = model.TopicId;
                var qdb = new QuestionContext();
                qdb.Questions.Add(q);
                qdb.SaveChanges();
                //Saves Answers to database
                for (int i = 0; i < 5; i++)
                {
                    //question.options[i].QuestionId = question.QuestionId;
                    model.Answers[i].IsCorrect = (model.Answer == i) ? true : false; 
                    //send ans to database
                    model.Answers[i].QuestionId = q.QuestionId;
                    adb.Answers.Add(model.Answers[i]);

                }
                adb.SaveChanges();
                
                return RedirectToAction("Index");
            }

            return View(model);
            
        }

        //
        // GET: /Topic/Edit/5

        public ActionResult Edit(int id = 0)
        {
            QuestionModel question = qdb.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }


        [HttpPost]
        public ActionResult Edit(QuestionModel question)
        {
            if (ModelState.IsValid)
            {
                qdb.Entry(question).State = EntityState.Modified;
                qdb.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        //
        // GET: /Topic/Delete/5

        public ActionResult Delete(int id = 0)
        {
            QuestionModel question = qdb.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        //
        // POST: /Topic/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id = 0)
        {
            QuestionModel question = qdb.Questions.Find(id);

            var qCount = qdb.Questions.Where(q => q.TopicId == question.TopicId).Count();

            var testTopics = new TestTopicsContext().TestTopics.ToList().Where(t => t.TopicId == question.TopicId);

            foreach (TestTopics tt in testTopics)
            { 
                if(tt.Quantity >= qCount)
                    throw new Exception("Question cannot be deleted because only " + qCount + " questions for it's topic exist and at least that many are required for 1 or more tests.");
            }
            

            var answered = new AnsweredQuestionsContext().AnsweredQuestions.ToList().Where(a => a.questionId == question.QuestionId);
            if (answered.Count() > 0)
            {
                throw new Exception("Question cannot be deleted because it has been answered " + answered.Count() + " time(s)");
               // ModelState.AddModelError("", "Question cannot be deleted because it has been answered " + answered.Count() + " time(s)");
                //return View();
            }


            var answers = adb.Answers.ToList();
            foreach(AnswerModel a in answers)
            {
                if(a.QuestionId == id)
                {
                    adb.Answers.Remove(a);
                }
                    
            }
            adb.SaveChanges();

            qdb.Questions.Remove(question);
            qdb.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            qdb.Dispose();
            base.Dispose(disposing);
        }

    }
}

