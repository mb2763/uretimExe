using Dapper;
using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.UretimOperasyonlar
{
    public class UretimOperasyonHareketDal : BaseDal<UretimOperasyonHareket>, IUretimOperasyonHareketDal
    {
        public UretimOperasyonHareketDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<UretimOperasyonHareket> GetViewListWhere(string whereSql)
        {
            var sql = UretimOperasyonHareket.GetSelectSqlCode(whereSql);
            var data = Connection.Query<UretimOperasyonHareket>(sql);
            return data;
        }
    }
}