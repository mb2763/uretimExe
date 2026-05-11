using My.Core.Data;
using My.Entities.SmsAyarlar;
using System.Data;

namespace My.DataAccess.SmsAyarlar {
    public class SmsAyarDal : BaseDal<SmsAyar>, ISmsAyarDal {
        public SmsAyarDal(IDbConnection connection) : base(connection) {
        }
    }
}
