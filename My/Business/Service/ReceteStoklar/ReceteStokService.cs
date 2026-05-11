using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteStoklar;
using My.Entities.ReceteStoklar;

namespace My.Business.Service.ReceteStoklar
{
    public class ReceteStokService : BaseService<ReceteStok>, IReceteStokService
    {
        public ReceteStokService(IReceteStokDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}