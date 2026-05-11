using Dapper;
using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.MikroModul
{
    public class MikroSiparisDal : BaseDal<MikroSiparis>, IMikroSiparisDal
    {
        public MikroSiparisDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<MikroSiparis> GetViewListWhere(string whereSql = "")
        {
            var sql =
                @" Select * From( SELECT TOP (100) PERCENT MIN(sip_Guid) AS SipGuid,sip_evrakno_seri  as EvrakSeri , sip_evrakno_sira  as EvrakSira , sip_belgeno as BelgeNo, 
sip_musteri_kod as  CariKodu , dbo.fn_CarininIsminiBul(0, sip_musteri_kod)as  CariUnvani , 
 (SELECT        fir_unvan  FROM            dbo.FIRMALAR  WHERE        (fir_sirano = dbo.SIPARISLER.sip_firmano)) AS FirmaUnvan, 
sip_tarih As Tarih , MIN(sip_teslim_tarih) AS TeslimTarihi ,SUM(sip_miktar) as Miktar ,  
dbo.fn_TalepTemin(sip_tip) AS SiparisTip, dbo.fn_SiparisCins(sip_cins) AS SiparisCins , 
dbo.fn_SiparisAcikKapali(sip_kapat_fl, SUM(sip_miktar), SUM(sip_teslim_miktar)) AS SiparisAcikKapali  
/*, SUM(sip_teslim_miktar)as TeslimMiktar, 
SUM(dbo.fn_Evrak_Kalan_Miktar(sip_miktar, sip_teslim_miktar, sip_kapat_fl)) AS KalanMiktar, SUM(sip_tutar) AS Tutar, COUNT(sip_satirno) AS sip_satirno, 
dbo.fn_TalepTemin(sip_tip) AS sip_tip, dbo.fn_SiparisCins(sip_cins) AS SiparisCins ,
dbo.fn_CariSektorIsmi(sip_musteri_kod) AS CariSektorIsmi,dbo.fn_CariGrupIsmi(sip_musteri_kod) AS CariGrupIsmi, dbo.fn_CariBolgeIsmi(sip_musteri_kod) AS CariBolgeIsmi, 
dbo.fn_SiparisAcikKapali(sip_kapat_fl, SUM(sip_miktar), SUM(sip_teslim_miktar)) AS SiparisAcikKapali, sip_depono AS sip_depono, sip_cins , sip_durumu  , 
 CASE WHEN SUM(CAST(sip_cagrilabilir_fl AS smallint)) = COUNT(sip_satirno) THEN dbo.fn_EvetHayir(1) ELSE dbo.fn_EvetHayir(0) END AS sip_cagrilabilir  */
FROM            dbo.SIPARISLER WITH (NOLOCK)
WHERE   (sip_tip = 0 and   sip_cins= 0 ) 
GROUP BY sip_tarih, sip_tip, sip_cins, sip_evrakno_seri, sip_evrakno_sira, sip_musteri_kod, sip_kapat_fl, sip_firmano, sip_depono, sip_durumu, sip_belgeno,sip_harekettipi 
ORDER BY sip_tarih, sip_tip, sip_cins, sip_evrakno_seri, sip_evrakno_sira, sip_musteri_kod, sip_kapat_fl, sip_firmano, sip_depono,sip_harekettipi ) Sip  " +
                whereSql;

            var data = Connection.Query<MikroSiparis>(sql);

            return data;
        }
    }
}