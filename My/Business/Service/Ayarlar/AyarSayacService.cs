using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Ayarlar;
using My.Entities.Ayarlar;

namespace My.Business.Service.Ayarlar
{
    public class AyarSayacService : BaseService<AyarSayac>, IAyarSayacService
    {
        public AyarSayacService(IAyarSayacDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
        }
    }
}