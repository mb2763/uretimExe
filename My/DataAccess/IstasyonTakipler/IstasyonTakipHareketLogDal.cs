using Dapper;
using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.IstasyonTakipler
{
    public class IstasyonTakipHareketLogDal : BaseDal<IstasyonTakipHareketLog>, IIstasyonTakipHareketLogDal
    {
        public IstasyonTakipHareketLogDal(IDbConnection connection) : base(connection)
        {


        }
        /// <summary>
        /// Log Tablo LG  Hareket Tablo HR
        /// </summary>
        /// <param name="whereSql"></param>
        /// <returns></returns>
        public IEnumerable<IstasyonTakipHareketLog> GetViewListWhere(string whereSql)
        {
           /* var sql =
                @"  select LG.*,TH.ReceteKodu,TH.ReceteAdi,TH.OperasyonKodu,TH.OperasyonAdi,TH.IstasyonKodu,TH.IstasyonAdi  from IstasyonTakipHareketLog LG  
LEFT OUTER JOIN IstasyonTakipHareket TH ON TH.Id=LG.IstHrId     " + whereSql + "  ";*/
 var sql =
                @"  select LG.*,TH.ReceteKodu,TH.ReceteAdi,TH.OperasyonKodu,TH.OperasyonAdi,TH.IstasyonKodu,TH.IstasyonAdi 
,Ur.IsEmriNo,Ur.SiparisKodu AS IsEmriKodu,K.Adi+' '+K.Soyadi as  Kullanici,TH.PlanlananMiktar,Th.UretimMiktari,TH.Durumu
 from IstasyonTakipHareketLog LG  
LEFT OUTER JOIN IstasyonTakipHareket TH ON TH.Id=LG.IstHrId
  LEFT OUTER JOIN UretimEmri UR ON LG.UrId = UR.Id
  LEFT OUTER JOIN Personel K ON LG.KayitEden =K.Kodu      " + whereSql + "  ";

            var data = Connection.Query<IstasyonTakipHareketLog>(sql);
            return data;
        }
    }

}
