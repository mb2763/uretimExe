using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteIstasyonGruplar;
using My.Entities.ReceteIstasyonGruplar;

namespace My.Business.Service.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupOperasyonService : BaseService<ReceteIstasyonGrupOperasyon>, IReceteIstasyonGrupOperasyonService
    {
        private IReceteIstasyonGrupOperasyonDal _dal;
        private ILogManager _ilogger;

        public ReceteIstasyonGrupOperasyonService(IReceteIstasyonGrupOperasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
