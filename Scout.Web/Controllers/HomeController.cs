using Scout.BusinessLayer;
using Scout.BusinessLayer.Result;
using Scout.DataAccessLayer.EntityFramework;
using Scout.Entities;
using Scout.Entities.ValueObject;
using Scout.Web.Filter;
using Scout.Web.Models;
using Scout.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace Scout.Web.Controllers
{
    public class HomeController : Controller
    {
        ShareManager shareManager = new ShareManager();
        FootballerManager footballerManager = new FootballerManager();
        ManagerManager managerm = new ManagerManager();
        AdminManager adminmanager = new AdminManager();

        // GET: Home

        //Ana Sayfa işlemleri


        public ActionResult ShowShare()
        {

            return View("ShowShare", shareManager.ListQueryable().OrderByDescending(x => x.CreatedDate).ToList());
        }

        public ActionResult MostLiked()
        {
            return View("ShowShare", shareManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult Index()
        {
            return View();
        }

        //Futbolcu Kullanıcı İşlemleri
        public ActionResult FootballerRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FootballerRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Footballer> res = footballerManager.FootballerRegister(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkVM notifyObj = new OkVM()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/FootballerLogin",
                    RedirectingTimeout = 3000
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden paylaşım yapamazsınız");
                return View("Ok", notifyObj);
            }
            return View(model);
        }
        public ActionResult FootballerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FootballerLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Footballer> res = footballerManager.LoginFootballer(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                CurrentSession.Set<Footballer>("login", res.Result);
                if (CurrentSession.footballer.CountryId == null || CurrentSession.footballer.ProvinceId == null || CurrentSession.footballer.FootId == null
                    || CurrentSession.footballer.PositionId == null || CurrentSession.footballer.OtherPositionId == null)
                {
                    return RedirectToAction("EditFootballerProfile");
                }
                return RedirectToAction("ShowFootballerProfile");
            }

            return View(model);
        }

        public ActionResult ShowFootballerProfile()
        {
            var shares = shareManager.ListQueryable().Include("Owner").Where(
            x => x.Owner.Id == CurrentSession.footballer.Id).OrderByDescending(
            x => x.CreatedDate);

            return View(shares.ToList());
        }
        public ActionResult Photos()
        {
            var shares = shareManager.ListQueryable().Include("Owner").Where(
               x => x.Owner.Id == CurrentSession.footballer.Id).OrderByDescending(
               x => x.CreatedDate);

            return View(shares.ToList());
        }
        public ActionResult Videos()
        {
            var shares = shareManager.ListQueryable().Include("Owner").Where(
               x => x.Owner.Id == CurrentSession.footballer.Id).OrderByDescending(
               x => x.CreatedDate);

            return View(shares.ToList());
        }
        public ActionResult FootballerProfileInformations()
        {
            BusinessLayerResult<Footballer> res = footballerManager.GetFootballerById(CurrentSession.footballer.Id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditFootballerProfile(int? id)
        {           
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", CurrentSession.footballer.CountryId);
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName", CurrentSession.footballer.ProvinceId);
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", CurrentSession.footballer.FootId);
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", CurrentSession.footballer.PositionId);
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", CurrentSession.footballer.OtherPositionId);

            BusinessLayerResult<Footballer> res = footballerManager.GetFootballerById(CurrentSession.footballer.Id);
            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                    RedirectingUrl = "Home/EditFootballerProfile"
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditFootballerProfile(Footballer footballer, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                         (ProfileImage.ContentType == "image/jpeg" ||
                         ProfileImage.ContentType == "image/jpg" ||
                         ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"footballer_{footballer.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    footballer.ProfileImageFileName = filename;
                }
                BusinessLayerResult<Footballer> res = footballerManager.UpdateFootballerProfile(footballer);

                if (res.Errors.Count > 0)
                {
                    ErrorVM errorNotifyObj = new ErrorVM()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/FootballerProfileInformations"
                    };
                    return View("Error", errorNotifyObj);
                }
                //Profil güncellendiği için Session güncellendi..
                CurrentSession.Set<Footballer>("login", res.Result);
                
                ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", footballer.CountryId);
                ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "Country", "ProvinceName", footballer.ProvinceId);
                ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", footballer.FootId);
                ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", footballer.PositionId);
                ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", footballer.OtherPositionId);
                return RedirectToAction("EditFootballerProfile");
            }
            return View(footballer);
        }
        public ActionResult DeleteFootballerProfile()
        {
            BusinessLayerResult<Footballer> res = footballerManager.RemoveFootballerById(CurrentSession.footballer.Id);
            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/FootballerProfileInformations"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult FootballerActivate(Guid id)
        {
            BusinessLayerResult<Footballer> res = footballerManager.ActivateFootballer(id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkVM okNotifyObj = new OkVM()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/FootballerLogin"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık paylaşım yapabilirsiniz.");

            return View("Ok", okNotifyObj);
        }
        public ActionResult ByFootballer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Footballer footballer = footballerManager.ListQueryable().Where(x => x.Id == id).First();

            return View("FootballerProfileInformations", footballer);
        }

        [HttpPost]
        public ActionResult Paylas(Share share)
        {

            if (ModelState.IsValid)
            {
                share.Owner = CurrentSession.footballer;
                share.ModifiedDate = DateTime.Now;
                share.CreatedDate = DateTime.Now;
                shareManager.Insert(share);
                return RedirectToAction("ShowFootballerProfile", "Home");
            }

            return View(share);
        }




        //Menajer Kullanıcı İşlemleri
        public ActionResult ManagerRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManagerRegister(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Manager> res = managerm.ManagerRegister(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                OkVM notifyObj = new OkVM()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/ManagerLogin"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden paylaşım yapamazsınız");
                return View("Ok", notifyObj);
            }
            return View(model);
        }


        public ActionResult ManagerActivate(Guid id)
        {
            BusinessLayerResult<Manager> res = managerm.ActivateManager(id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }

            OkVM okNotifyObj = new OkVM()
            {
                Title = "Hesap Aktifleştirildi",
                RedirectingUrl = "/Home/ManagerLogin"
            };

            okNotifyObj.Items.Add("Hesabınız aktifleştirildi. Artık aradığınız kriterlere göre futbolcu bulabilir, paylaşımlara yorum yapabilir ve beğenebilirsiniz.");

            return View("Ok", okNotifyObj);
        }

        public ActionResult ManagerLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ManagerLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                BusinessLayerResult<Manager> res = managerm.LoginManager(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                CurrentSession.Set<Manager>("loginm", res.Result);
                return RedirectToAction("ShowShare");
            }

            return View(model);
        }
        public ActionResult ManagerProfileInformations()
        {
            BusinessLayerResult<Manager> res = managerm.GetManagerById(CurrentSession.manager.Id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/ShowShare",
                    RedirectingTimeout = 3000
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        public ActionResult EditManagerProfile()
        {
            BusinessLayerResult<Manager> res = managerm.GetManagerById(CurrentSession.manager.Id);

            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/ManagerProfileInformations"
                };
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }

        [HttpPost]
        public ActionResult EditManagerProfile(Manager manager, HttpPostedFileBase ProfileImage)
        {
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                         (ProfileImage.ContentType == "image/jpeg" ||
                         ProfileImage.ContentType == "image/jpg" ||
                         ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"manager_{manager.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    manager.ProfileImageFileName = filename;
                }
                BusinessLayerResult<Manager> res = managerm.UpdateManagerProfile(manager);

                if (res.Errors.Count > 0)
                {
                    ErrorVM errorNotifyObj = new ErrorVM()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenemedi",
                        RedirectingUrl = "/Home/ManagerProfileInformations",

                    };
                    return View("Error", errorNotifyObj);
                }
                //Profil güncellendiği için Session güncellendi..
                CurrentSession.Set<Manager>("loginm", res.Result);
                return RedirectToAction("ManagerProfileInformations");
            }
            return View(manager);

        }
        public ActionResult DeleteManagerProfile()
        {
            BusinessLayerResult<Manager> res = managerm.RemoveManagerById(CurrentSession.manager.Id);
            if (res.Errors.Count > 0)
            {
                ErrorVM errorNotifyObj = new ErrorVM()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowShare"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult ByManager(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            Manager manager = managerm.ListQueryable().Where(x => x.Id == id).First();

            return View("ManagerProfileInformations", manager);
        }
        [AuthManagerLogin]
        public ActionResult FilterSearch()
        {
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName");
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName");
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName");
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName");
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName");
            return View();
        }

        [HttpPost]
        public ActionResult FilterSearch(SearchModel footballer)
        {
            string countryID = Request.Form["CountryId"].ToString();
            int cID = Convert.ToInt32(countryID);
            string provinceID = Request.Form["ProvinceId"].ToString();
            int prID = Convert.ToInt32(provinceID);
            string positionID = Request.Form["PositionId"].ToString();
            int poID = Convert.ToInt32(positionID);
            string otherPositionID = Request.Form["OtherPositionId"].ToString();
            int opID = Convert.ToInt32(otherPositionID);
            string footID = Request.Form["FootId"].ToString();
            int fID = Convert.ToInt32(footID);

            List<Footballer> ftsdb = footballerManager.ListQueryable().ToList();
            List<Footballer> fts = new List<Footballer>();
            foreach (Footballer f in ftsdb)
            {
                if (f.CountryId == cID && f.ProvinceId == prID && f.PositionId == poID && f.OtherPositionId == opID && f.FootId == fID)
                {
                    if (f.Age >= footballer.MinAge && f.Age <= footballer.MaxAge && f.Height >= footballer.MinHeight && f.Height <= footballer.MaxHeight
                        && f.Weight >= footballer.MinWeight && f.Weight <= footballer.MaxHeight)
                    {
                        fts.Add(f);
                    }
                }

                ViewBag.futbolcular = fts;

            }
            ViewBag.CountryId = new SelectList(CacheHelper.GetCountriesFromCache(), "CountryId", "CountryName", footballer.Country);
            ViewBag.ProvinceId = new SelectList(CacheHelper.GetProvincesFromCache(), "ProvinceId", "ProvinceName", footballer.Province);
            ViewBag.FootId = new SelectList(CacheHelper.GetFootsFromCache(), "FootId", "FootName", footballer.Foot);
            ViewBag.PositionId = new SelectList(CacheHelper.GetPositionsFromCache(), "PositionId", "PositionName", footballer.Position);
            ViewBag.OtherPositionId = new SelectList(CacheHelper.GetOtherPositionsFromCache(), "OtherPositionId", "OtherPositionName", footballer.OtherPosition);
            return View(footballer);


        }


        //Admin Giriş
        public ActionResult Adminstator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Adminstator(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                BusinessLayerResult<Admin> res = adminmanager.LoginAdmin(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }
                CurrentSession.Set<Admin>("logina", res.Result);
                return RedirectToAction("AdminPage");
            }

            return View(model);
        }
        [AuthLoginAdmin]
        public ActionResult AdminPage()
        {
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");

        }




    }
}