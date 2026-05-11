using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.UretimAciklamalar;
using My.Entities.UretimAciklamalar;

namespace My.Business.Service.UretimAciklamalar
{
    public class AciklamaDegerService : BaseService<AciklamaDeger>, IAciklamaDegerService
    {
        private IAciklamaDegerDal _dal;
        private ILogManager _ilogger;
        public AciklamaDegerService(IAciklamaDegerDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
