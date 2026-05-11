using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Receteler;
using My.Entities.Receteler;

namespace My.Business.Service.Receteler
{
    public class ReceteDetayService : BaseService<ReceteDetay>, IReceteDetayService
    {
        public ReceteDetayService(IReceteDetayDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}