using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteGruplar;
using My.Entities.ReceteGruplar;

namespace My.Business.Service.ReceteGruplar
{
    public class ReceteGrupService : BaseService<ReceteGrup>, IReceteGrupService
    {
        public ReceteGrupService(IReceteGrupDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}