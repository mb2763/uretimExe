using My.Core.Data;
using System; 

namespace My.Entities.StokDepoRaflar {
    [Table("StokDepoRaf")]
    public class StokDepoRaf {
        [Key]
        public int Id { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string CariKodu { get; set; }
        public string CariAdi { get; set; }
        public string Parti { get; set; }
        public int LotNo { get; set; }
        public DateTime? SKT { get; set; }
        public int DepoNo { get; set; }
        public string DepoAdi { get; set; }
        public string Raf { get; set; }
        public double Miktar { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public DateTime? HareketTarihi { get; set; }
        public string HareketAciklamasi { get; set; }
        public string IsEmriNo { get; set; }
        public string IsEmriKodu { get; set; }


        public static string GetInsertSqlCode() {
            string sql = @"  IF EXISTS
  (SELECT * FROM  StokDepoRaf  WHERE Id = @Id)
    UPDATE  StokDepoRaf SET
      StokKodu             = @StokKodu          ,   
      StokAdi              = @StokAdi           ,   
      CariKodu             = @CariKodu          ,   
      CariAdi              = @CariAdi           ,   
      Parti                = @Parti             ,
      LotNo                = @LotNo             ,
      SKT                  = @SKT               ,
      DepoNo               = @DepoNo            ,
      DepoAdi              = dbo.fn_DepoIsmi(@DepoNo)           ,
      Raf                  = @Raf               ,
      Miktar               = @Miktar            ,
      KayitTarihi          = @KayitTarihi       ,     
      HareketTarihi        = @HareketTarihi     ,     
      HareketAciklamasi    = @HareketAciklamasi ,   
      IsEmriNo             = @IsEmriNo          ,
      IsEmriKodu           = @IsEmriKodu      
    WHERE  Id = @Id
  ELSE
  INSERT INTO  StokDepoRaf  (
   StokKodu           ,
   StokAdi            ,
   CariKodu           ,
   CariAdi            ,
   Parti              ,
   LotNo              ,
   SKT                ,
   DepoNo             ,
   DepoAdi            ,
   Raf                ,
   Miktar             ,
   KayitTarihi        ,
   HareketTarihi      ,
   HareketAciklamasi  ,
   IsEmriNo,IsEmriKodu)
  VALUES(
   @StokKodu          ,
   @StokAdi           ,
   @CariKodu          ,
   @CariAdi           ,
   @Parti             ,
   @LotNo             ,
   @SKT               ,
   @DepoNo            ,
   dbo.fn_DepoIsmi(@DepoNo)  ,
   @Raf               ,
   @Miktar            ,
   @KayitTarihi       ,
   @HareketTarihi     ,
   @HareketAciklamasi ,
   @IsEmriNo,@IsEmriKodu );";
            return sql;
        }
    }
}
