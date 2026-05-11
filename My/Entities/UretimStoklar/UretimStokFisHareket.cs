using My.Core.Data;
using System;

namespace My.Entities.UretimStoklar {

    [Table("UretimStokFisHareket")]
    public class UretimStokFisHareket { 
        public UretimStokFisHareket Clone() { return (UretimStokFisHareket)MemberwiseClone(); }
        [Key] public Guid? Id { get; set; }
        public Guid? FisId { get; set; }
        public int GirisCikis { get; set; }
        public DateTime? Tarih { get; set; }
        public int SatirNo { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double GirisMiktar { get; set; }
        public double CikisMiktar { get; set; }
        public string Birimi { get; set; }
        public short IrsBirimPntr { get; set; }
        public string PartiNo { get; set; }
        public int LotNo { get; set; }
        public DateTime? UretimTarihi { get; set; }
        public DateTime? SonKulTarihi { get; set; }
        public int Sira { get; set; }
        public string BarGui { get; set; }
        public Guid? IrsHGuid { get; set; }
        public Guid? StGuid { get; set; }
        public short Sil { get; set; }
        public string TakipKodu1 { get; set; }
        public string TakipKodu2 { get; set; }
        public string TakipKodu3 { get; set; }

        [NotMapped]
        public string Kontrol { get; set; }

        public UretimStokFisHareket() {
            Id = Guid.Empty;
            FisId = Guid.Empty;
            Sil = 0;
            GirisMiktar = 0;
            CikisMiktar = 0;
            Sira = 0;
            BarGui = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            TakipKodu1 = "";
            TakipKodu2 = "";
            TakipKodu3 = "";

        }

        public static string GetSelectSqlCode(string sor = "") {
            string sql = @"SELECT *,'' as Kontrol FROM UretimStokFisHareket  WITH (NOLOCK) " + sor;
            return sql;
        }
        public static string GetSelectSqlCodeByFisId(Guid? fisId) {
            string sql = $@"SELECT *,'' as Kontrol FROM UretimStokFisHareket  WITH (NOLOCK) where  FisId='{fisId}'";
            return sql;
        }
        public static string GetInsertSqlCode() {
            string sql = @"  IF EXISTS
  (SELECT * FROM dbo.UretimStokFisHareket  WHERE Id = @Id )
    UPDATE dbo.UretimStokFisHareket SET
      FisId = @FisId,
      GirisCikis = @GirisCikis,
      Tarih = @Tarih,
      SatirNo = @SatirNo,
      StokKodu = @StokKodu,
      StokAdi = @StokAdi,
      GirisMiktar = @GirisMiktar,
      CikisMiktar = @CikisMiktar,
      Birimi = @Birimi,
      IrsBirimPntr = @IrsBirimPntr,
      PartiNo = @PartiNo,
      LotNo = @LotNo,
      UretimTarihi = @UretimTarihi, 
      SonKulTarihi = @SonKulTarihi,
      Sira = @Sira,
      BarGui = @BarGui,
      IrsHGuid = @IrsHGuid,
      StGuid = @StGuid,
      Sil = @Sil,
      TakipKodu1 = @TakipKodu1,
      TakipKodu2 = @TakipKodu2,
      TakipKodu3 = @TakipKodu3 
    WHERE Id = @Id
  ELSE
    INSERT INTO dbo.UretimStokFisHareket  (
      Id,
      FisId,
      GirisCikis,
      Tarih,
      SatirNo,
      StokKodu,
      StokAdi,  
      GirisMiktar,  
      CikisMiktar,  
      Birimi,  
      IrsBirimPntr,  
      PartiNo,  
      LotNo, 
      UretimTarihi,
      SonKulTarihi,
      BarGui, 
      IrsHGuid, 
      StGuid, 
      Sil, 
      TakipKodu1, 
      TakipKodu2, 
      TakipKodu3  
     )
    VALUES(
      @Id,
      @FisId,
      @GirisCikis,
      @Tarih,
      @SatirNo,
      @StokKodu,  
      @StokAdi,  
      @GirisMiktar,  
      @CikisMiktar,  
      @Birimi,  
      @IrsBirimPntr,  
      @PartiNo,  
      @LotNo, 
      @UretimTarihi,
      @SonKulTarihi,
      @BarGui,
      @IrsHGuid,
      @StGuid,
      @Sil,
      @TakipKodu1,
      @TakipKodu2,
      @TakipKodu3  
      );";
            return sql;
        }
    }
}
