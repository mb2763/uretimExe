using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.IstasyonBakimlar;
using My.Entities.IstasyonBakimlar;

namespace My.Business.Service.IstasyonBakimlar
{
    public class IstasyonBakimService : BaseService<IstasyonBakim>, IIstasyonBakimService
    {
        private IIstasyonBakimDal _dal;
        private ILogManager _ilogger;

        public IstasyonBakimService(IIstasyonBakimDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
