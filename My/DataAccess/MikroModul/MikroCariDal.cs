using Dapper;
using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.MikroModul
{
    public class MikroCariDal : BaseDal<MikroCari>, IMikroCariDal
    {
        public MikroCariDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<MikroCari> GetViewListWhere(string whereSql)
        {
            var sql = @"  
                       Select TOP (100) PERCENT C.cari_Guid As CrGuid, C.cari_kod As CariKodu,   
                       C.cari_unvan1  as CariUnvani1 ,  C.cari_unvan2 as CariUnvani2, 
	                   C.cari_Ana_cari_kodu as AnaCariKodu,C.cari_grup_kodu as GrupKodu,C.cari_sektor_kodu as SektorKodu,
                       dbo.fn_DovizSembolu(C.cari_doviz_cinsi) AS Dvz ,
                       coalesce(CHU.Kargo ,'') as Kargo, 
                       coalesce(C.cari_EMail,'') AS Email,
                       coalesce(CHU.FASON ,'') as Fason
                       FROM dbo.CARI_HESAPLAR C left outer JOIN CARI_HESAPLAR_USER CHU on C.cari_Guid = CHU.Record_uid   " + whereSql;

            var data = Connection.Query<MikroCari>(sql);

            return data;
        }
    }
}