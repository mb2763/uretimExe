using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteIstasyonGruplar;
using My.Entities.ReceteIstasyonGruplar;

namespace My.Business.Service.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupIstasyonService : BaseService<ReceteIstasyonGrupIstasyon>, IReceteIstasyonGrupIstasyonService
    {
        private IReceteIstasyonGrupIstasyonDal _dal;
        private ILogManager _ilogger;

        public ReceteIstasyonGrupIstasyonService(IReceteIstasyonGrupIstasyonDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
