using My.Core.Data;
using My.Entities.MailAyarlar;
using System.Data;

namespace My.DataAccess.MailAyarlar
{
    public class MailAyarDal : BaseDal<MailAyar>, IMailAyarDal
    {
        public MailAyarDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
