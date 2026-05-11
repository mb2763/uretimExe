using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.SmsAyarlar;
using My.Entities.SmsAyarlar;

namespace My.Business.Service.SmsAyarlar {
    public class SmsAyarService : BaseService<SmsAyar>, ISmsAyarService {
        public SmsAyarService(ISmsAyarDal dal, ILogManager ilogger) : base(dal, ilogger) {
        }
    }
}
