using Dapper;
using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.IstasyonTakipler
{
    public class IstasyonTakipHareketDal : BaseDal<IstasyonTakipHareket>, IIstasyonTakipHareketDal
    {
        public IstasyonTakipHareketDal(IDbConnection connection) : base(connection)
        {


        }

        public IEnumerable<IstasyonTakipHareket> GetViewListWhere(string whereSql)
        {
            var sql =
                @"  select *  from IstasyonTakipHareket  " + whereSql + "  ";

            var data = Connection.Query<IstasyonTakipHareket>(sql);
            return data;
        }
    }
}
