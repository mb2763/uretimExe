using Dapper;
using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.MikroModul
{
    public class MikroSiparisHareketDal : BaseDal<MikroSiparisHareket>, IMikroSiparisHareketDal
    {
        public MikroSiparisHareketDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<MikroSiparisHareket> GetViewListWhere(string whereSql = "")
        {
            var where = whereSql;
            var sql = $@"SELECT TOP (100) PERCENT   sip_evrakno_seri as EvrakSeri, 
                        sip_evrakno_sira as EvrakSira,  sip_stok_kod  as StokKodu ,
  CASE WHEN sip_harekettipi = 0 THEN dbo.fn_StokIsmi(sip_stok_kod) WHEN sip_harekettipi = 1 THEN dbo.fn_CarininIsminiBul(3, sip_stok_kod) 
  WHEN sip_harekettipi = 2 THEN dbo.fn_CarininIsminiBul(5, sip_stok_kod) WHEN sip_harekettipi = 3 THEN dbo.fn_CarininIsminiBul(8, sip_stok_kod) END AS StokAdi,
  sip_miktar as Miktar  ,S.sto_birim1_ad aS Birim,
					    dbo.fn_TalepTemin(sip_tip) AS TalepTemin,  
						dbo.fn_Split(dbo.fn_GetResourceMax('A', 3033, DEFAULT), sip_harekettipi + 1, DEFAULT) AS HareketTipi,
						dbo.fn_SiparisCins(sip_cins) AS SiparisCins,  sip_tarih as Tarih , dbo.fn_DateTimeKontrol(sip_teslim_tarih) AS TeslimTarihi,    dbo.fn_Evrak_Kalan_Miktar(sip_miktar, 
                        sip_teslim_miktar, sip_kapat_fl) AS Evrak_Kalan_Miktar, sip_musteri_kod as CariKodu , sip_aciklama as Aciklama  , sip_depono as DepoNo , dbo.fn_DepoIsmi(sip_depono) AS DepoIsmi,
						sip_birim_pntr as SipBirimPntr, sip_Guid  as SipHGuid 
FROM     SIPARISLER SP LEFT OUTER JOIN STOKLAR S ON SP.sip_stok_kod = S.sto_kod 
   {where}
  /*WHERE  (sip_teslim_miktar > 0) AND (sip_kapat_fl = 0) AND (sip_cagrilabilir_fl = 1)*/
ORDER BY sip_teslim_tarih, sip_tarih, sip_evrakno_seri, sip_evrakno_sira ";

            var data = Connection.Query<MikroSiparisHareket>(sql);

            return data;
        }

        public IEnumerable<MikroSiparisHareket> GetViewListSeriSira(string seri, string sira)
        {
            var where = $@"WHERE sip_tip = 0 and ( sip_evrakno_seri = '{seri}' and sip_evrakno_sira = {sira})";
            var sql = $@" SELECT TOP (100) PERCENT   sip_evrakno_seri as EvrakSeri, 
                        sip_evrakno_sira as EvrakSira,  sip_stok_kod  as StokKodu ,
  CASE WHEN sip_harekettipi = 0 THEN dbo.fn_StokIsmi(sip_stok_kod) WHEN sip_harekettipi = 1 THEN dbo.fn_CarininIsminiBul(3, sip_stok_kod) 
  WHEN sip_harekettipi = 2 THEN dbo.fn_CarininIsminiBul(5, sip_stok_kod) WHEN sip_harekettipi = 3 THEN dbo.fn_CarininIsminiBul(8, sip_stok_kod) END AS StokAdi,
  sip_miktar as Miktar  ,S.sto_birim1_ad aS Birim,
					    dbo.fn_TalepTemin(sip_tip) AS TalepTemin,  
						dbo.fn_Split(dbo.fn_GetResourceMax('A', 3033, DEFAULT), sip_harekettipi + 1, DEFAULT) AS HareketTipi,
						dbo.fn_SiparisCins(sip_cins) AS SiparisCins,  sip_tarih as Tarih , dbo.fn_DateTimeKontrol(sip_teslim_tarih) AS TeslimTarihi,    dbo.fn_Evrak_Kalan_Miktar(sip_miktar, 
                        sip_teslim_miktar, sip_kapat_fl) AS Evrak_Kalan_Miktar, sip_musteri_kod as CariKodu , sip_aciklama as Aciklama  , sip_depono as DepoNo , dbo.fn_DepoIsmi(sip_depono) AS DepoIsmi,
						sip_birim_pntr as SipBirimPntr, sip_Guid  as SipHGuid 
FROM     SIPARISLER SP LEFT OUTER JOIN STOKLAR S ON SP.sip_stok_kod = S.sto_kod 
   {where}
  /*WHERE  (sip_teslim_miktar > 0) AND (sip_kapat_fl = 0) AND (sip_cagrilabilir_fl = 1)*/
ORDER BY sip_teslim_tarih, sip_tarih, sip_evrakno_seri, sip_evrakno_sira ";

            var data = Connection.Query<MikroSiparisHareket>(sql);

            return data;
        }

        public IEnumerable<MikroSiparisHareket> GetViewListWhere1111111111111111(string whereSql = "")
        {
            var where = whereSql;
            var sql = $@"  SELECT   dbo.fn_GirisCikis(SH.sth_tip) AS Tip,  EA.SHEvrIsim AS EvrakIsmi, 
 Sip.sip_evrakno_seri AS EvrakSeri,  Sip.sip_evrakno_sira AS EvrakSira , SH.sth_stok_kod AS StokKodu, 
 dbo.fn_StokIsmi(SH.sth_stok_kod) AS StokAdi,    SH.sth_miktar AS Miktar,   S.sto_birim1_ad aS Birim,  
 dbo.fn_VerilenBirimMiktarHesapla(SH.sth_stok_kod,SH.sth_miktar,2) AS Miktar2,
 S.sto_birim2_ad aS Birim2 ,Sip.sip_Guid As SipGuid,SH.sth_Guid as SipHGuid
 FROM STOK_HAREKETLERI SH left outer join SIPARISLER Sip ON sth_sip_uid=sip_Guid INNER JOIN
 STOKLAR S ON SH.sth_stok_kod = S.sto_kod INNER JOIN vw_Stok_Hareket_Evrak_Isimleri EA ON SH.sth_evraktip = EA.SHEvrNo
   {whereSql}
 /*  where SH.sth_evraktip = 1 and SH.sth_tip = 1  */
 ORDER BY   SH.sth_tarih ,SH.sth_evrakno_seri,SH.sth_evrakno_sira,SH.sth_stok_kod
 /* SH.sth_tarih AS TARİH, SH.sth_evrakno_seri AS [EVRAK SERİ],SH.sth_evrakno_sira AS [EVRAK SIRA],  
 SH.sth_cari_kodu AS [CARİ KOD],   dbo.fn_CarininIsminiBul(0, SH.sth_cari_kodu) AS [CARİ İSMİ], */   ";

            var data = Connection.Query<MikroSiparisHareket>(sql);

            return data;
        }
    }
}