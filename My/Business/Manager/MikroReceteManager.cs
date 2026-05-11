using My.Core.Result;
using My.Entities.Models;
using System.Collections.Generic;

namespace My.Business.Manager
{
    public class MikroReceteManager
    {
        private readonly DatabaseFactoryMikro _dbMikro;
        private DatabaseFactoryPro _dbPro;

        public MikroReceteManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro)
        {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
        }

        public IDataResult<IEnumerable<MikroReceteModel>> GetMikroReceteList(string sorgu)
        {
            var sql = @" SELECT TOP 100 PERCENT
            rec_anakod AS ReceteKodu,
            dbo.fn_StokIsmi(rec_anakod) as ReceteAdi,
            dbo.fn_StokBirimi(rec_anakod, rec_anabirim) as AnaBirimi,  
            rec_anamiktar as AnaMiktar
            ,s.sto_pasif_fl AS StokAktif /*0 pasif 1 aktif*/ 
            FROM URUN_RECETELERI R WITH(NOLOCK) 
            LEFT OUTER JOIN STOKLAR S ON s.sto_kod =R.rec_anakod 
            " + sorgu + @"  
            GROUP BY rec_anakod,rec_anabirim,rec_anamiktar,s.sto_pasif_fl 
            ORDER BY rec_anakod ;";
            return _dbMikro.GenelServis.Query<MikroReceteModel>(sql, null);//,rec_fireyuzde as FireYuzde
        }
     
        public IDataResult<IEnumerable<MikroStokStandartMaliyetModel>> GetStokStandartMaliyetler(string stokKodlari)
        {
            var sql = @" select s.sto_kod AS StokKodu ,coalesce(s.sto_standartmaliyet,0) AS StandartMaliyet  from STOKLAR s   WITH (NOLOCK)  
            where 1=1 and s.sto_kod in(" + stokKodlari +") ";
            return _dbMikro.GenelServis.Query<MikroStokStandartMaliyetModel>(sql, null);
        }
      public IDataResult<IEnumerable<MikroStokSonAlisSatisFiyatlar>> GetStokSonAlisSatisFiyatlar(string stokKodlari)
        {
            var sql = @" 
    SELECT TOP 100 PERCENT
     sto_kod AS StokKodu  ,
     sto_isim AS StokAdi  ,  
     dbo.fn_by_Stok_Son5_Giris_Fiyati(sto_kod,NULL,0) AS [SonAlis],
     dbo.fn_by_Stok_Son5_Giris_Fiyati(sto_kod,NULL,1) AS [Son5AlisOrtalama] , 
     sto_standartmaliyet AS StandartMaliyet ,
     dbo.fn_by_Stok_Son5_Giris_Fiyati(sto_kod,NULL,2) AS [SonSayimGiris] , 
     dbo.fn_by_Stok_Son5_Giris_Fiyati(sto_kod,NULL,3) AS [DevirGiris]    
    FROM dbo.STOKLAR WITH (NOLOCK)   where 1=1 and  sto_kod in(" + stokKodlari +@")  ORDER BY sto_kod   ";
            return _dbMikro.GenelServis.Query<MikroStokSonAlisSatisFiyatlar>(sql, null);
        }

        public IDataResult<IEnumerable<MikroReceteHareketModel>> GetMikroReceteHareketler(string receteKodu)
        {
            var sql = @" 
        select rec_anakod as ReceteKodu,dbo.fn_StokIsmi(rec_anakod) as ReceteAdi,
        dbo.fn_StokBirimi(rec_anakod, rec_anabirim) as AnaBirimi,
        rec_anamiktar as AnaMiktar,
        CASE WHEN  rec_tuketim_tur =  0 THEN 'Stok' ELSE 'Diger' END AS Turu,
        rec_tuketim_kod as StokKodu,
        dbo.fn_StokIsmi(rec_tuketim_kod) as StokAdi,
        rec_tuketim_miktar as Miktar,
        dbo.fn_StokBirimi(rec_tuketim_kod, rec_tuketim_birim) as Birimi,
        rec_satirno as Sira,
        dbo.fn_DepoIsmi(rec_depono) as Depo,
        rec_create_date as ReceteTarihi,
        rec_Guid as ReceteGuid ,
        rec_fireyuzde as FireYuzde
        from  dbo.URUN_RECETELERI WITH(NOLOCK)
            where rec_iptal=0 and rec_anakod ='" + receteKodu + @"' ;";
            return _dbMikro.GenelServis.Query<MikroReceteHareketModel>(sql, null);
        }
    }
}