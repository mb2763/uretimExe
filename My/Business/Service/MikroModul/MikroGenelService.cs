
using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.Entities.Geneller;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;

namespace My.Business.Service.MikroModul {
    public class MikroGenelService : BaseServiceCore<Genel>, IMikroGenelService {
        public MikroGenelService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) {

        }
        public IDataResult<IEnumerable<string>> GrupListesi(string tabloadi, string sutun) {
            var sql = " Select " + sutun + " as Kodu From " + tabloadi + " group by " + sutun + ";";
            var rs = Query<string>(sql, null);
            return rs;
        }

        public IDataResult<IEnumerable<MikroDepo>> GetMikroDepoListesi(string where) {
            var sql = " Select  dep_no as DepoNo, dep_adi as DepoAdi From   DEPOLAR  " + where ;
            var rs = Query<MikroDepo>(sql, null);
            return rs;
        }

    }
}
