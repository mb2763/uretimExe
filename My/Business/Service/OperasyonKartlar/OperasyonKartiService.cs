using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.OperasyonKartlar;
using My.Entities.OperasyonKartlar;

namespace My.Business.Service.OperasyonKartlar
{
    public class OperasyonKartiService : BaseService<OperasyonKarti>, IOperasyonKartiService
    {
        private IOperasyonKartiDal _dal;
        private ILogManager _ilogger;

        public OperasyonKartiService(IOperasyonKartiDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

      
    }
}