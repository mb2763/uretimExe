using Dapper;
using My.Core.Data;
using My.Entities.Receteler;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.Receteler
{
    public class ReceteAnaDal : BaseDal<ReceteAna>, IReceteAnaDal
    {
        public ReceteAnaDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<ReceteAna> GetListWhere(string where)
        {
            var sql = @" Select  * ,
(SELECT count(*) FROM ReceteOperasyon WHERE RcAId=ReceteAna.Id) AS OperasyonAdet,
(SELECT count(*) FROM ReceteIstasyon WHERE RcAId=ReceteAna.Id) AS IstasyonAdet
From ReceteAna  " + where;

            return Connection.Query<ReceteAna>(sql);
        }
    }
}