using Dapper;
using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.UretimOperasyonlar
{
    public class UretimOperasyonHareketDetayDal : BaseDal<UretimOperasyonHareketDetay>, IUretimOperasyonHareketDetayDal
    {
        public UretimOperasyonHareketDetayDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<UretimOperasyonHareketDetay> GetViewListWhere(string whereSql)
        {
            var sql = UretimOperasyonHareketDetay.GetSelectSqlCode(whereSql);
            var data = Connection.Query<UretimOperasyonHareketDetay>(sql);
            return data;
        }
    }
}