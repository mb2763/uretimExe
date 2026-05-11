using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteIstasyonlar;
using My.Entities.ReceteIstasyonlar;

namespace My.Business.Service.ReceteIstasyonlar
{
    public class ReceteIstasyonCariService : BaseService<ReceteIstasyonCari>, IReceteIstasyonCariService
    {
        private IReceteIstasyonCariDal _dal;
        private ILogManager _ilogger;

        public ReceteIstasyonCariService(IReceteIstasyonCariDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}