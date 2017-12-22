using Scout.BusinessLayer.Abstract;
using Scout.BusinessLayer.Result;
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
   public class AdminManager : ManagerBase<Admin>
    {
        public BusinessLayerResult<Admin> LoginAdmin(LoginViewModel data)
        {
            BusinessLayerResult<Admin> res = new BusinessLayerResult<Admin>();
            Admin admin = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (admin != null)
            {
                res.Result = admin;
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı ve ya parola yanlış");
            }
            return res;
        }

    }
}
