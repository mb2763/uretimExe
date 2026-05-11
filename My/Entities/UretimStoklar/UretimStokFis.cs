using My.Core.Data;
using System;

namespace My.Entities.UretimStoklar
{
    [Table("UretimStokFis")]
    public class UretimStokFis
    {
        [Key] public Guid? Id { get; set; }
        public Guid? UrId { get; set; }
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
            string sql = @"SELECT * FROM UretimStokFis  WITH (NOLOCK) " + sor;
            return sql;
        }
        public static string GetSelectSqlCodeById(Guid id)
        {
            string sql = $@"SELECT * FROM UretimStokFis  WITH (NOLOCK) where  Id='{id}'";
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
        public static string GetOnayUpdSqlCode(Guid? id, string durumu, string usercode)
        {
            string sql = $@"UPDATE UretimStokFis  SET Durumu='{durumu}',Degistiren='{usercode}', DegistirmeTarihi= GETDATE() where  Id='{id}'";
            return sql;
        }

    }
}
