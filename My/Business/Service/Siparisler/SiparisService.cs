using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Siparisler;
using My.Entities.Siparisler;

namespace My.Business.Service.Siparisler
{
    public class SiparisService : BaseService<Siparis>, ISiparisService
    {
        public SiparisService(ISiparisDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}