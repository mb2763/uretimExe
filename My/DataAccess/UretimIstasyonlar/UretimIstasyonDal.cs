using Dapper;
using My.Core.Data;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.UretimIstasyonlar
{
    public class UretimIstasyonDal : BaseDal<UretimIstasyon>, IUretimIstasyonDal
    {
        public UretimIstasyonDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<UretimIstasyon> GetViewListWhere(string whereSql)
        {
            var sql = UretimIstasyon.GetSelectSqlCode(whereSql);
            var data = Connection.Query<UretimIstasyon>(sql);
            return data;
        }
    }
}