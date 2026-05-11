using Dapper;
using My.Core.Data;
using My.Entities.Siparisler;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.Siparisler
{
    public class SiparisHareketDal : BaseDal<SiparisHareket>, ISiparisHareketDal
    {
        public SiparisHareketDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<SiparisHareketModel> GetViewListWhere(string whereSql)
        {
            var sql = @" 
              SELECT SH.Id, S.SiparisKodu, S.CariKodu, S.CariUnvani, S.Tarih, S.TeslimTarihi, SH.ReceteGrupKodu, SH.ReceteKodu, 
              SH.ReceteAdi, SH.StokKodu, SH.StokAdi, SH.Miktar, 
              SH.UretimMiktari, SH.FireMiktari, SH.IptalMiktari 
              COALESCE(SH.Miktar,0)-(COALESCE(SH.UretimMiktari,0)+COALESCE(SH.FireMiktari,0)+COALESCE(SH.IptalMiktari,0)) as KalanMiktar , 
              SH.Renk, SH.Birim, SH.Aciklama, SH.SipId,   SH.RcAId 
              FROM   SiparisHareket SH LEFT JOIN Siparis S  ON S.Id = SH.SipId  " + whereSql + "; ";
            var data = Connection.Query<SiparisHareketModel>(sql);
            return data;
        }
          public IEnumerable<SiparisHareket> GetViewListWhere2(string whereSql)
        {
            var sql = @" 
              SELECT SH.* ,S.Tarih, S.TeslimTarihi
              FROM   SiparisHareket SH LEFT JOIN Siparis S  ON S.Id = SH.SipId  " + whereSql + "; ";
            var data = Connection.Query<SiparisHareket>(sql);
            return data;
        }
        public IEnumerable<SiparisHareket> GetViewListKalanMiktarliWhere(string whereSql)
        {
        //    var sql11111 = @" 
        //            SELECT SH.Id, S.SiparisKodu,  S.Tarih, S.TeslimTarihi, SH.ReceteGrupKodu, SH.ReceteKodu, 
        //      SH.ReceteAdi, SH.StokKodu, SH.StokAdi, SH.Miktar,  
        //      COALESCE( 
        //      ( SELECT  KalanMiktar  FROM UretimOperasyon UO WHERE UO.SipHId=Sh.Id  AND  UO.RcOId=
        //      ( SELECT    Id  FROM ReceteOperasyon RO WHERE RO.RcAId=UO.RcAId and  RO.Sira=( 
        //        SELECT  Max(Coalesce(RO2.Sira,0)) FROM ReceteOperasyon RO2   WHERE  RO2.RcAId= UO.RcAId )
        //      ) ),  SH.Miktar) AS KalanMiktar,  
        //      SH.Birim,SH.Renk,SH.Beden, SH.Aciklama,  SH.Parti, SH.Lot,SH.SipId,   SH.RcAId,
        //      SH.Ent, SH.EntCode, SH.EntDate,  SH.EntSeri, SH.EntSira, SH.EntGuid, SH.EntKayitGuid , SH.EntKayitSeri, SH.EntKayitSira,
        //      SH.UretimMiktari, SH.FireMiktari, SH.IptalMiktari 
        //      FROM   SiparisHareket SH
        //      LEFT JOIN Siparis S  ON S.Id = SH.SipId     " + whereSql + "; ";

            var sql = @" 
                    SELECT SH.Id, S.SiparisKodu,  S.Tarih, S.TeslimTarihi, SH.ReceteGrupKodu, SH.ReceteKodu, 
              SH.ReceteAdi, SH.StokKodu, SH.StokAdi, SH.Miktar, 
              SH.UretimMiktari, SH.FireMiktari, SH.IptalMiktari ,
              COALESCE(SH.Miktar,0)-(COALESCE(SH.UretimMiktari,0)+COALESCE(SH.FireMiktari,0)+COALESCE(SH.IptalMiktari,0)) as KalanMiktar ,  
              SH.Birim,SH.Renk,SH.Beden, SH.Aciklama,  SH.Parti, SH.Lot,SH.SipId,   SH.RcAId,
              SH.Ent, SH.EntCode, SH.EntDate,  SH.EntSeri, SH.EntSira, SH.EntGuid, SH.EntKayitGuid , SH.EntKayitSeri, SH.EntKayitSira 
              
              FROM   SiparisHareket SH
              LEFT JOIN Siparis S  ON S.Id = SH.SipId     " + whereSql + "; ";
            var data = Connection.Query<SiparisHareket>(sql);
            return data;
        }
    }
}