using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.UretimTalepler;
using My.Entities.UretimTalepler;

namespace My.Business.Service.UretimTalepler
{
    public class UretimTalepService : BaseService<UretimTalep>, IUretimTalepService
    {
        private readonly IUretimTalepDal _dal;
        private readonly ILogManager _ilogger;

        public UretimTalepService(IUretimTalepDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }


    }
}