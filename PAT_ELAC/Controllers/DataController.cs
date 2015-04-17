using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAT_ELAC.Models;

namespace PAT_ELAC.Controllers
{
    public class DataController : Controller
    {
        private AnsweredQuestionsContext aqc = new AnsweredQuestionsContext();

        //
        // GET: /Data/

        public ActionResult Index()
        {
            return View(aqc.AnsweredQuestions.ToList());
        }

    }
}
