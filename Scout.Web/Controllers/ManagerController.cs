using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scout.Entities;
using Scout.Web.Models;
using Scout.BusinessLayer;
using Scout.BusinessLayer.Result;
using Scout.Web.Filter;

namespace Scout.Web.Controllers
{
  
    public class ManagerController : Controller
    {
        private ManagerManager managerM = new ManagerManager();
     
        public ActionResult Index()
        {
            var footballer = managerM.List();
            return View(footballer);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = managerM.Find(x=>x.Id==id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Manager manager)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Manager> res = managerM.Insert(manager);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(manager);
                }
                return RedirectToAction("Index");
            }

            return View(manager);
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = managerM.Find(x => x.Id == id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Manager manager)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Manager> res = managerM.Update(manager);
                if (res.Errors.Count > 0)
                {

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(manager);
                }
                return RedirectToAction("Index");
            }
            return View(manager);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager manager = managerM.Find(x => x.Id == id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            return View(manager);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manager manager = managerM.Find(x => x.Id == id);
            return RedirectToAction("Index");
        }

       
    }
}
