using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.ReceteGruplar;
using My.Entities.ReceteGruplar;

namespace My.Business.Service.ReceteGruplar
{
    public class ReceteGrupDetayService : BaseService<ReceteGrupDetay>, IReceteGrupDetayService
    {
        public ReceteGrupDetayService(IReceteGrupDetayDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}