using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.Ayarlar;
using My.Entities.Ayarlar;

namespace My.Business.Service.Ayarlar {
    public class AyarService : BaseService<Ayar>, IAyarService {
        public AyarService(IAyarDal dal, ILogManager ilogger) : base(dal, ilogger) {
        }
    }
}
