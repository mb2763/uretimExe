using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.UretimEmirler;
using My.Entities.UretimEmirler;

namespace My.Business.Service.UretimEmirler
{
    public class UretimEmriService : BaseService<UretimEmri>, IUretimEmriService
    {
        private IUretimEmriDal _dal;
        private ILogManager _ilogger;

        public UretimEmriService(IUretimEmriDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}