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
using System.IO;
using Scout.Web.Filter;

namespace Scout.Web.Controllers
{
    public class ShareController : Controller
    {
        ShareManager shareManager = new ShareManager();
        LikedManager likedManager = new LikedManager();
        public ActionResult Index()
        {
            var shares = shareManager.ListQueryable().Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.footballer.Id).OrderByDescending(
                x => x.CreatedDate);

            return View(shares.ToList());
        }
        
        public ActionResult MyLikedShares()
        {
         var shares = likedManager.ListQueryable().Include("LikedUser").
         Include("Share").Where
         (x => x.LikedUser.Id == CurrentSession.manager.Id).Select
         (x => x.Share).Include("Owner").OrderByDescending
         (x => x.CreatedDate);

          return View("Index", shares.ToList());

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = shareManager.Find(x => x.ShareId == id);
            if (share == null)
            {
                return HttpNotFound();
            }
            return View(share);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Share share, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                //image Upload
                string fileName = Path.GetFileNameWithoutExtension(image.FileName);
                string extension = Path.GetExtension(image.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmsfff") + extension;
                share.ShareImageFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/images/"), fileName);
                image.SaveAs(fileName);
                //if (image != null &&
                //        (image.ContentType == "image/jpeg" ||
                //        image.ContentType == "image/jpg" ||
                //        image.ContentType == "image/png"))
                //{
                //    string filename = $"footballerShare_{share.ShareId}.{image.ContentType.Split('/')[1]}";

                //    image.SaveAs(Server.MapPath($"~/images/{filename}"));
                //    share.ShareImageFileName = filename;

                //Video Upload

                


                share.Owner = CurrentSession.footballer;
                    share.ModifiedDate = DateTime.Now;
                    share.CreatedDate = DateTime.Now;

                    shareManager.Insert(share);
                    return RedirectToAction("ShowFootballerProfile", "Home");
                }

               
            
            return View(share);
        }

        public ActionResult VideoUpload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VideoUpload(Share share, HttpPostedFileBase video)
        {
            if (ModelState.IsValid)
            {
                //image Upload
                string fileName = Path.GetFileNameWithoutExtension(video.FileName);
                string extension = Path.GetExtension(video.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmsfff") + extension;
                share.ShareVideoFileName = fileName;
                fileName = Path.Combine(Server.MapPath("~/videos"), fileName);
                video.SaveAs(fileName);
               

                share.Owner = CurrentSession.footballer;
                share.ModifiedDate = DateTime.Now;
                share.CreatedDate = DateTime.Now;

                shareManager.Insert(share);
                return RedirectToAction("ShowFootballerProfile", "Home");
            }

            return View(share);
           
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = shareManager.Find(x => x.ShareId == id);
            if (share == null)
            {
                return HttpNotFound();
            }
            return View(share);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Share share)
        {
            ModelState.Remove("CreatedDate");
            ModelState.Remove("ModifiedDate");
            if (ModelState.IsValid)
            {
                Share db_share = shareManager.Find(x => x.ShareId == share.ShareId);
                db_share.ShareText = share.ShareText;
                db_share.ShareVideoFileName = share.ShareVideoFileName;
                db_share.ShareImageFileName = share.ShareImageFileName;

                shareManager.Update(db_share);

                return RedirectToAction("Index");
            }
            return View(share);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Share share = shareManager.Find(x => x.ShareId == id);
            if (share == null)
            {
                return HttpNotFound();
            }
            return View(share);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Share share = shareManager.Find(x => x.ShareId == id);
            shareManager.Delete(share);
            return RedirectToAction("Index");
        }
        [AuthManagerLogin]
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            List<int> likedShareeIds = likedManager.List
                (x => x.LikedUser.Id == CurrentSession.manager.Id && ids.Contains
                (x.Share.ShareId)).Select(x => x.Share.ShareId)
                .ToList();

            return Json(new { result = likedShareeIds });
        }
        [HttpPost]
        public ActionResult SetLikeState(int shareid, bool liked)
        {
            int res = 0;

            Liked like = likedManager.Find(x => x.Share.ShareId == shareid && x.LikedUser.Id == CurrentSession.manager.Id);

            Share share = shareManager.Find(x => x.ShareId == shareid);

            if(like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if(like == null && liked == true)
            {
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.manager,
                    Share = share
                });
            }
            if(res > 0)
            {
                if (liked)
                {
                    share.LikeCount++;
                }
                else
                {
                    share.LikeCount--;
                }
                shareManager.Update(share);
                return Json(new { hasError = false, errorMessage = string.Empty, result = share.LikeCount });
            }
            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi", result = share.LikeCount });
        }
    }
}
