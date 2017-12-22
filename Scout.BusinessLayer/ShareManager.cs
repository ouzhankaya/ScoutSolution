using Scout.BusinessLayer.Abstract;
using Scout.BusinessLayer.Result;
using Scout.DataAccessLayer.EntityFramework.EntityFramework;
using Scout.Entities;
using Scout.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scout.BusinessLayer
{
    public class ShareManager : ManagerBase<Share>
    {
        public BusinessLayerResult<Share> UploadImage(Share data)
        {
            BusinessLayerResult<Share> res = new BusinessLayerResult<Share>();

            res.Result = Find(x => x.ShareId == data.ShareId);

            res.Result.ShareImageFileName = data.ShareImageFileName;
            if (string.IsNullOrEmpty(data.ShareImageFileName) == false)
            {
                res.Result.ShareImageFileName = data.ShareImageFileName;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ImageCouldNotUploaded, "Resim Yüklenemedi");
            }
            return res;

        }
    }
}