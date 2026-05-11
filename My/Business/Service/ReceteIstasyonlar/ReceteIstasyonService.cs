using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteIstasyonlar;
using My.Entities.ReceteIstasyonlar;

namespace My.Business.Service.ReceteIstasyonlar
{
    public class ReceteIstasyonService : BaseService<ReceteIstasyon>, IReceteIstasyonService
    {
        private IReceteIstasyonDal _dal;
        private ILogManager _ilogger;

        public ReceteIstasyonService(IReceteIstasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}