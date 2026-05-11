using My.Core.Data;
using System; 

namespace My.Entities.UretimStoklar
{
    [Table("UretimStokFis")]
    public class UretimStokFisModel
    {
        public UretimStokFisModel Clone() { return (UretimStokFisModel)MemberwiseClone(); }
        [Key] public Guid? Id { get; set; }
        public Guid? UrId { get; set; }
        public string IsEmriNo { get; set; }
        public string IsEmriKodu { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public DateTime? Tarih { get; set; }
        public string Durumu { get; set; }
        public string EvrakNo { get; set; }
        public string BelgeNo { get; set; }
        public string Personel { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }


        public static string GetSelectSqlCode(string sor = "")
        {
            string sql = @" SELECT UF.*,Ur.IsEmriNo,Ur.SiparisKodu AS IsEmriKodu,UrI.IstasyonKodu,UrI.IstasyonAdi
FROM dbo.UretimStokFis UF
LEFT OUTER JOIN UretimEmri UR ON UF.UrId = UR.Id
LEFT OUTER JOIN UretimOperasyon UrO ON UrO.UrId = Ur.Id AND UrO.Sira = 1
left outer join UretimIstasyon UrI on UrI.UrOId = UrO.Id " + sor;
            return sql;
        }
        public static string GetSelectSqlCodeById(Guid id)
        {
            string sql = $@"SELECT UF.*,Ur.IsEmriNo,Ur.SiparisKodu AS IsEmriKodu,UrI.IstasyonKodu,UrI.IstasyonAdi
FROM dbo.UretimStokFis UF
LEFT OUTER JOIN UretimEmri UR ON UF.UrId = UR.Id
LEFT OUTER JOIN UretimOperasyon UrO ON UrO.UrId = Ur.Id AND UrO.Sira = 1
left outer join UretimIstasyon UrI on UrI.UrOId = UrO.Id  where  UF.Id='{id}'";
            return sql;
        }
        public static string GetInsertSqlCode()
        {
            string sql = @"  IF EXISTS
  (SELECT * FROM dbo.UretimStokFis  WHERE Id = @Id)
    UPDATE dbo.UretimStokFis SET
      UrId = @UrId,
      Tarih = @Tarih,
      Durumu = @Durumu,
      EvrakNo = @EvrakNo,
      BelgeNo = @BelgeNo,
      Personel = @Personel,
      KayitEden = @KayitEden, 
      KayitTarihi = @KayitTarihi,
      Degistiren = @Degistiren,
      DegistirmeTarihi = @DegistirmeTarihi  
    WHERE Id = @Id
  ELSE
    INSERT INTO dbo.UretimStokFis  (
      Id,
      UrId,
      Tarih,
      Durumu,  
      EvrakNo,  
      BelgeNo,  
      Personel, 
      KayitEden,
      KayitTarihi,
      Degistiren, 
      DegistirmeTarihi  
     )
    VALUES(
      @Id,
      @UrId,
      @Tarih,
      @Durumu,  
      @EvrakNo,  
      @BelgeNo,  
      @Personel, 
      @KayitEden,
      @KayitTarihi,
      @Degistiren,
      @DegistirmeTarihi  
      );";
            return sql;
        }

        //        public static UretimStokFis GetFis(UretimStokFisModel _mdl)
        //        {
        //            UretimStokFis mdl = new UretimStokFis();
        //            mdl.Id = _mdl.Id;
        //            mdl.UrId = _mdl.UrId; 
        //            mdl.Tarih = _mdl.Tarih;
        //            mdl.Durumu = _mdl.Durumu;
        //            mdl.EvrakNo = _mdl.EvrakNo;
        //            mdl.BelgeNo = _mdl.BelgeNo;
        //            mdl.Personel = _mdl.Personel; 
        //            mdl.KayitEden = _mdl.KayitEden;
        //            mdl.KayitTarihi = _mdl.KayitTarihi;
        //            mdl.Degistiren = _mdl.Degistiren;
        //            mdl.DegistirmeTarihi = _mdl.DegistirmeTarihi; 
        //            return mdl;
        //        }

        //        private string GetSelectSql(string where) {
        //            string sql = $@"
        //SELECT 
        // Ur.IsEmriNo , UrO.ReceteKodu,UrO.ReceteAdi,UrStF.Tarih,
        // COALESCE( UrStF.Durumu   ,'Beklemede') AS Durumu, 
        // COALESCE( UrStF.EvrakNo  ,''         ) AS EvrakNo , 
        // COALESCE( UrStF.BelgeNo  ,''         ) AS BelgeNo,
        // COALESCE( UrStF.Personel ,''         ) AS Personel, 
        // COALESCE(UrStF.Id,0x0) AS Id, 
        // COALESCE(UrStF.UrId,Ur.Id) AS UrId, 
        // UrStF.KayitEden,  UrStF.KayitTarihi,  UrStF.Degistiren,   UrStF.DegistirmeTarihi  , 
        // Ur.Durumu AS UrtDurumu, 
        // Ur.SiparisKodu AS UrtKodu,
        // Ur.BaslangicTarihi AS UrtBasTarihi
        //FROM UretimEmri Ur {where}
        //LEFT OUTER JOIN UretimStokFis UrStF ON Ur.Id = UrStF.UrId
        //LEFT OUTER JOIN UretimOperasyon UrO ON UrO.UrId = Ur.Id AND UrO.Sira = 1
        // ";

        //            return sql;
        //        }

    }
}
