using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Receteler;
using My.Entities.Receteler;

namespace My.Business.Service.Receteler
{
    public class ReceteOperasyonService : BaseService<ReceteOperasyon>, IReceteOperasyonService
    {
        private IReceteOperasyonDal _dal;
        private ILogManager _ilogger;

        public ReceteOperasyonService(IReceteOperasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}