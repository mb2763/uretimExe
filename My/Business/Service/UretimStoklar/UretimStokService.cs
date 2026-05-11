using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.UretimStoklar;
using My.Entities.UretimStoklar;

namespace My.Business.Service.UretimStoklar
{
    public class UretimStokService : BaseService<UretimStok>, IUretimStokService
    {
        private IUretimStokDal _dal;
        private ILogManager _ilogger;

        public UretimStokService(IUretimStokDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}