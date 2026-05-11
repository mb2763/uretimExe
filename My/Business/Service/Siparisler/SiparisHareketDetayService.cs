using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Siparisler;
using My.Entities.Siparisler;

namespace My.Business.Service.Siparisler
{
    public class SiparisHareketDetayService : BaseService<SiparisHareketDetay>, ISiparisHareketDetayService
    {
        public SiparisHareketDetayService(ISiparisHareketDetayDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}