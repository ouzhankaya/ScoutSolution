using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Scout.Entities;
using Scout.BusinessLayer;
using Scout.BusinessLayer.Result;
using Scout.Web.Models;
using Scout.Web.Filter;

namespace Scout.Web.Controllers
{


    public class FootballerController : Controller
    {
        private FootballerManager footballerManager = new FootballerManager();
        private CountryManager countryManager = new CountryManager();
        public ActionResult Index()
        {
            var footballers = footballerManager.ListQueryable().Include("Country").Include("Province").Include("Foot").Include("Position").Include("OtherPosition");
            return View(footballers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = footballerManager.Find(x => x.Id == id);
            if (footballer == null)
            {
                return HttpNotFound();
            }
            return View(footballer);
        }

        public ActionResult Create()
        {
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(),"CountryId" , "CountryName");
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName");
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName");
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName");
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Footballer footballer)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Footballer> res = footballerManager.Insert(footballer);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(footballer);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", footballer.CountryId);
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName", footballer.ProvinceId);
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", footballer.FootId);
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", footballer.PositionId);
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", footballer.OtherPositionId);
            return View(footballer);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = footballerManager.Find(x => x.Id == id);
            if (footballer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", footballer.CountryId);
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName", footballer.ProvinceId);
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", footballer.FootId);
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", footballer.PositionId);
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", footballer.OtherPositionId);
            return View(footballer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Footballer footballer)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Footballer> res = footballerManager.Update(footballer);
                if (res.Errors.Count > 0)
                {
                   
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(footballer);
                }
                return RedirectToAction("Index");
            }
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", footballer.CountryId);
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName", footballer.ProvinceId);
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", footballer.FootId);
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", footballer.PositionId);
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", footballer.OtherPositionId);
            return View(footballer);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Footballer footballer = footballerManager.Find(x => x.Id == id);
            if (footballer == null)
            {
                return HttpNotFound();
            }
            return View(footballer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Footballer footballer = footballerManager.Find(x => x.Id == id);
            footballerManager.Delete(footballer);
            return RedirectToAction("Index");
        }
    }
}
