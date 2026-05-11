using My.Core.Data;
using System;

namespace My.Entities.IstasyonTakipler {

    [Table("IstasyonTakipHareket")]
    public class IstasyonTakipHareket {
        [Key] public Guid? Id { get; set; }
        public Guid UrId { get; set; }
        public Guid UrIId { get; set; }
        public string Durumu { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public string SiparisKodu { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double KalanMiktar { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public bool Fason { get; set; }
        public string FasonCariKodu { get; set; }
        public string FasonCariUnvani { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public string TalepEden { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; } 
        public bool OlcumZorunlu { get; set; }
        public double OlcumDegeriMin { get; set; }
        public double OlcumDegeriMax { get; set; } 
        public double OlcumDegeri { get; set; }

        public static string GetMiktarFireUpdateSqlCode(Guid? istHrId) {
            //  MamulGiris, FireMamulGiris, UretimBitis,
            string sql = $@" 
DECLARE @urtMiktar FLOAT; 
DECLARE @fireMiktar FLOAT;
DECLARE @iptalMiktar FLOAT;
DECLARE @istHrId UNIQUEIDENTIFIER;
SET @istHrId='{istHrId}';
SELECT @urtMiktar=COALESCE(sum(COALESCE(Miktar,0)),0)   ,@fireMiktar=COALESCE(sum(COALESCE(FireMiktar,0)),0) ,@iptalMiktar=COALESCE(sum(COALESCE(IptalMiktar,0)),0)   FROM IstasyonTakipHareketDetay
 WHERE IstHrId =@istHrId and (Turu='MamulGiris' or Turu='FireMamulGiris' or Turu ='UretimBitis' or Turu ='UretimIptal' ) ;
 UPDATE IstasyonTakipHareket SET UretimMiktari= COALESCE(  @urtMiktar,0),FireMiktari= COALESCE(@fireMiktar,0),IptalMiktari= COALESCE(@iptalMiktar,0) WHERE Id=@istHrId;
 UPDATE IstasyonTakipHareket SET KalanMiktar =  COALESCE(PlanlananMiktar,0) -  (COALESCE(UretimMiktari,0) + COALESCE(IptalMiktari,0)+ COALESCE(FireMiktari,0) )    WHERE Id=@istHrId; ";
            return sql;
        }

        public static string GetIptalUpdateSqlCodeByUrIId(Guid? urIId) {
            //  MamulGiris, FireMamulGiris, UretimBitis,
            string sql = $@" 
DECLARE @iptalMiktar FLOAT; 
DECLARE @plnMiktar FLOAT;   
DECLARE @urIId UNIQUEIDENTIFIER; 
SET @urIId='{urIId}';  
SELECT @plnMiktar= COALESCE(sum(COALESCE(PlanlananMiktar,0)),0) , @iptalMiktar=COALESCE(sum(COALESCE(IptalMiktari,0)),0)   FROM IstasyonTakipHareket  WHERE UrIId =@urIId   ;
IF (@iptalMiktar>0 AND @plnMiktar > 0)  
BEGIN  
	UPDATE IstasyonTakipStokHareket SET IptalMiktari = (PlanlananMiktar / @plnMiktar) * @iptalMiktar    WHERE UrIId =@urIId and PlanlananMiktar > 0 ;
END

";
            return sql;
        }
    }
}
