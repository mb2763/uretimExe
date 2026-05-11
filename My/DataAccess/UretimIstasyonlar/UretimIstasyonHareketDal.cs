using Dapper;
using My.Core.Data;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.UretimIstasyonlar
{
    public class UretimIstasyonHareketDal : BaseDal< UretimIstasyonHareket>, IUretimIstasyonHareketDal
    {
        public UretimIstasyonHareketDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable< UretimIstasyonHareket> GetViewListWhere(string whereSql)
        {
            var sql = UretimIstasyonHareket.GetSelectSqlCode(whereSql);
            var data = Connection.Query< UretimIstasyonHareket>(sql);
            return data;
        }
    }
}