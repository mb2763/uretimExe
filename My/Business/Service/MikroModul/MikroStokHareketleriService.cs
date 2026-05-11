using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.MikroModul;
using My.Entities.Mikro;

namespace My.Business.Service.MikroModul
{
    public class MikroStokHareketleriService : BaseService<MikroStokHareketleri>, IMikroStokHareketleriService
    {
        private IMikroStokHareketleriDal _dal;
        private ILogManager _ilogger;

        public MikroStokHareketleriService(IMikroStokHareketleriDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}