using My.Core.Data;
using My.Core.Logger;
using My.DataAccess.MailAyarlar;
using My.Entities.MailAyarlar;

namespace My.Business.Service.MailAyarlar
{
    public class MailAyarService : BaseService<MailAyar>, IMailAyarService
    {
        private IMailAyarDal _dal;
        private ILogManager _ilogger;

        public MailAyarService(IMailAyarDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }
    }
}
