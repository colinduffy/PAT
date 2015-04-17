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
    public class ResourceController : Controller
    {
        private ResourceContext db = new ResourceContext();
        //private Test2Context tDb = new Test2Context();

        //
        // GET: /Resource/

        public ActionResult Index()
        {
            return View(db.Resources.ToList());
        
        }

        public ActionResult Test()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Test(String model)
        {
            //tDb.Values.Add(model);
            //tDb.SaveChanges();
            return RedirectToAction("Index");

        }

        //
        // GET: /Resource/Details/5

        public ActionResult Details(int id = 0)
        {
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        //
        // GET: /Resource/Create

        public ActionResult Create()
        {
            var topics = new TopicContext().Topics.ToList();
            var model = new Resource();
            model.Topics = new SelectList(topics, "TopicId", "description");
            return View(model);
        }

        //
        // POST: /Resource/Create

        [HttpPost]
        public ActionResult Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(resource);
        }

        //
        // GET: /Resource/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            var topics = new TopicContext().Topics.ToList();
            resource.Topics = new SelectList(topics, "TopicId", "description");
            return View(resource);
        }

        //
        // POST: /Resource/Edit/5

        [HttpPost]
        public ActionResult Edit(Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resource);
        }

        //
        // GET: /Resource/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        //
        // POST: /Resource/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}