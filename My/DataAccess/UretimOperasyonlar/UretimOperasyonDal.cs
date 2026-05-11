using Dapper;
using My.Core.Data;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.UretimOperasyonlar
{
    public class UretimOperasyonDal : BaseDal<UretimOperasyon>, IUretimOperasyonDal
    {
        public UretimOperasyonDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<UretimOperasyon> GetViewListWhere(string whereSql)
        {
            var sql = UretimOperasyon.GetSelectSqlCode(whereSql);
            var data = Connection.Query<UretimOperasyon>(sql);
            return data;
        }
    }
}