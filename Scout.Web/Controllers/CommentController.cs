using Scout.BusinessLayer;
using Scout.Entities;
using Scout.Web.Filter;
using Scout.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Scout.Web.Controllers
{


    public class CommentController : Controller
    {
        private ShareManager shareManager = new ShareManager();
        private CommentManager commentManager = new CommentManager();

        public ActionResult ShowShareComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = shareManager.ListQueryable().Include
                ("Comments").FirstOrDefault(x => x.ShareId == id);
            if (share == null)
            {
                HttpNotFound();
            }


            return PartialView("_PartialComments", share.Comments);
        }


        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.Find(x => x.CommentId == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.ModifiedDate = DateTime.Now;
            comment.CommentText = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.Find(x => x.CommentId == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }


            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]      
        public ActionResult Create(Comment comment, int? shareid)
        {
            if (ModelState.IsValid)
            {
                ModelState.Remove("CreatedDate");
                ModelState.Remove("ModifiededDate");


                if (shareid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Share share = shareManager.Find(x => x.ShareId == shareid);


                if (share == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Share = share;
                comment.Manager = CurrentSession.manager;
                comment.ModifiedDate = DateTime.Now;
                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

    }
}