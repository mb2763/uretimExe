using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.UretimAciklamalar;
using My.Entities.UretimAciklamalar;

namespace My.Business.Service.UretimAciklamalar
{
    public class AciklamaKodService : BaseService<AciklamaKod>, IAciklamaKodService
    {
        private IAciklamaKodDal _dal;
        private ILogManager _ilogger;

        public AciklamaKodService(IAciklamaKodDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
