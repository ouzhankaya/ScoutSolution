using Scout.BusinessLayer.Abstract;
using Scout.BusinessLayer.Result;
using Scout.Common.Helpers;
using Scout.DataAccessLayer.EntityFramework.EntityFramework;
using Scout.Entities;
using Scout.Entities.Messages;
using Scout.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.BusinessLayer
{
   public class ManagerManager : ManagerBase<Manager>
    {
       
        public BusinessLayerResult<Manager> LoginManager(LoginViewModel data)
        {
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
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
        public BusinessLayerResult<Manager> ManagerRegister(RegisterViewModel data)
        {
            Manager manager = Find(x => x.Username == data.Username);
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            if (manager != null)
            {
                if (manager.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (manager.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
            }

            else
            {
                int dbResult = base.Insert(new Manager()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    Password = data.Password,
                    Name = data.Name,
                    ActivateGuid = Guid.NewGuid(),
                    Lastname = data.Lastname,
                    DateOfBirth = Convert.ToDateTime(data.DateOfBirth),
                    ProfileImageFileName = "manager.png",
                });

                if (dbResult > 0)
                {
                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activateUri = $"{siteUri}/Home/ManagerActivate/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br>Hesabınızı aktifleştirmek için <a href='{activateUri}' target='_blank'>tıklayınız</a>.";

                    MailHelper.SendMail(body, res.Result.Email, "Scout Hesap Aktifleştirme");
                }
            }
            return res;
        }
        public BusinessLayerResult<Manager> ActivateManager(Guid activateId)
        {
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
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

        public BusinessLayerResult<Manager> GetManagerById(int id)
        {
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı.");
            }
            return res;
        }
        public BusinessLayerResult<Manager> UpdateManagerProfile(Manager data)
        {
            Manager db_manager = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            if (db_manager != null && db_manager.Id != data.Id)
            {
                if (db_manager.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_manager.Email == data.Email)
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
            res.Result.DateOfBirth = data.DateOfBirth;
            res.Result.CountOfViewProfile = data.CountOfViewProfile;
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
        public BusinessLayerResult<Manager> RemoveManagerById(int id)
        {
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            Manager manager = Find(x => x.Id == id);

            if (manager != null)
            {
                if (Delete(manager) == 0)
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
        public new BusinessLayerResult<Manager> Insert(Manager data)
        {
            Manager manager = Find(x => x.Username == data.Username);
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            res.Result = data;
            if (manager != null)
            {
                if (manager.Username == res.Result.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }
                if (manager.Email == res.Result.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-mail adresi kayıtlı");
                }
            }

            else
            {
                data.ProfileImageFileName = "user.png";
                data.ActivateGuid = Guid.NewGuid();
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.ManagerCouldNotInserted, "Menajer Eklenemedi");
                }
            }
            return res;
        }
        public new BusinessLayerResult<Manager> Update(Manager data)
        {
            Manager db_manager = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<Manager> res = new BusinessLayerResult<Manager>();
            res.Result = data;
            if (db_manager != null && db_manager.Id != data.Id)
            {
                if (db_manager.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı.");
                }

                if (db_manager.Email == data.Email)
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
            res.Result.CountOfViewProfile = data.CountOfViewProfile;
            res.Result.DateOfBirth = data.DateOfBirth;
            res.Result.IsActive = data.IsActive;

            if (string.IsNullOrEmpty(data.ProfileImageFileName) == false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ManagerCouldNotUpdated, "Menajer Güncellenemedi");
            }
            return res;
        }
    }
}
