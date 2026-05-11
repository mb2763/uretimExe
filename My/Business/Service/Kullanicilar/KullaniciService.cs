using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Kullanicilar;
using My.Entities.Kullanicilar;

namespace My.Business.Service.Kullanicilar
{
    public class KullaniciService : BaseService<Kullanici>, IKullaniciService
    {
        private IKullaniciDal _dal;
        private ILogManager _ilogger;

        public KullaniciService(IKullaniciDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}