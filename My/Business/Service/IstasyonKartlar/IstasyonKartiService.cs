using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.IstasyonKartlar;
using My.Entities.IstasyonKartlar;

namespace My.Business.Service.IstasyonKartlar
{
    public class IstasyonKartiService : BaseService<IstasyonKarti>, IIstasyonKartiService
    {
        private IIstasyonKartiDal _dal;
        private ILogManager _ilogger;

        public IstasyonKartiService(IIstasyonKartiDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}