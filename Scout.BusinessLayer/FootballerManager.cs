using Scout.DataAccessLayer.EntityFramework.EntityFramework;
using Scout.Entities;
using Scout.Entities.Messages;
using Scout.Entities.ValueObject;
using System;
using Scout.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scout.BusinessLayer.Result;
using Scout.BusinessLayer.Abstract;

namespace Scout.BusinessLayer
{
    public class FootballerManager : ManagerBase<Footballer>
    {

        public BusinessLayerResult<Footballer> FootballerRegister(RegisterViewModel data)
        {
            Footballer footballer = Find(x => x.Username == data.Username);
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            if (footballer != null)
            {
                if (footballer.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (footballer.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
                if(Convert.ToDateTime(data.DateOfBirth)> DateTime.Now)
                {
                    res.AddError(ErrorMessageCode.UpdatedDate, "Tarihi yanlış girdiniz");
                }
            }

            else
            {
                int dbResult = base.Insert(new Footballer()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    Name = data.Name,
                    ActivateGuid = Guid.NewGuid(),
                    Lastname = data.Lastname,
                    ProfileImageFileName = "user.png",
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth)
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/FootballerActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email, "Scout Hesap Aktifleştirme");
                }
            }
            return res;
        }
        public BusinessLayerResult<Footballer> LoginFootballer(LoginViewModel data)
        {
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);


            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktifleştirilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinizi kontrol ediniz.");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
            }
            return res;
        }
        public BusinessLayerResult<Footballer> ActivateFootballer(Guid activateId)
        {
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            res.Result = Find(x => x.ActivateGuid == activateId);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return res;
        }


        public BusinessLayerResult<Footballer> GetFootballerById(int id)
        {
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }
        public BusinessLayerResult<Footballer> UpdateFootballerProfile(Footballer data)
        {
            
            Footballer db_footballer = Find(x=>x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            if (db_footballer != null && db_footballer.Id != data.Id)
            {
                if (db_footballer.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_footballer.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Lastname = data.Lastname;
            res.Result.Password = data.Password;
            res.Result.PhoneNumber = data.PhoneNumber;
            res.Result.PositionId = data.PositionId;
            res.Result.OtherPositionId = data.OtherPositionId;
            res.Result.CountryId = data.CountryId;
            res.Result.FootId = data.FootId;
            res.Result.ProvinceId = data.ProvinceId;
            res.Result.Height = data.Height;
            res.Result.Weight = data.Weight;
            res.Result.DateOfBirth = data.DateOfBirth;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");
            }
            return res;
        }
        public BusinessLayerResult<Footballer> RemoveFootballerById(int id)
        {
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            Footballer footballer = Find(x => x.Id == id);

            if (footballer != null)
            {
                if (Delete(footballer) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }

        //Method Hiding
        public new BusinessLayerResult<Footballer> Insert(Footballer data)
        {
            Footballer footballer = Find(x => x.Username == data.Username);
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            res.Result = data;
            if (footballer != null)
            {
                if (footballer.Username == res.Result.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (footballer.Email == res.Result.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
            }

            else
            {
                data.ProfileImageFileName = "user.png";
                data.ActivateGuid = Guid.NewGuid();
                data.Username = res.Result.Username;
                data.DateOfBirth = Convert.ToDateTime(data.DateOfBirth);
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.FootballerCouldNotInserted, "Futbolcu Eklenemedi");
                }
            }
            return res;
        }
        public new BusinessLayerResult<Footballer> Update(Footballer data)
        {
            Footballer db_footballer = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<Footballer> res = new BusinessLayerResult<Footballer>();
            res.Result = data;
            if (db_footballer != null && db_footballer.Id != data.Id)
            {
                if (db_footballer.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_footballer.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta adresi kayıtlı.");
                }
                return res;
            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Username = data.Username;
            res.Result.Name = data.Name;
            res.Result.Lastname = data.Lastname;
            res.Result.Password = data.Password;
            res.Result.PhoneNumber = data.PhoneNumber;
            res.Result.PositionId = data.PositionId;
            res.Result.CountryId = data.CountryId;
            res.Result.FootId = data.FootId;
            res.Result.ProvinceId = data.ProvinceId;
            res.Result.Height = data.Height;
            res.Result.Weight = data.Weight;
            res.Result.DateOfBirth = data.DateOfBirth;
            res.Result.IsActive = data.IsActive;
            if (res.Result.Notification == null)
            {
                res.Result.Notification = new List<Notification>();
            }
            Notification not = new Notification() { NotificationText = "Profil Bilgileri Güncellendi", NotificationDate = DateTime.Now, Footballer = res.Result };
            res.Result.Notification.Add(not);

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.FootballerCouldNotUpdated, "Futbolcu Güncellenemedi");
            }
            return res;
        }
    }
}


