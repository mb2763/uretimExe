using My.Core.Data;
using System;

namespace My.Entities.DepoStoklar {
    [Table("DepoStokFis")]
    public class DepoStokFis {
        public DepoStokFis Clone() { return (DepoStokFis)MemberwiseClone(); }
        [Key] public Guid? Id { get; set; }
        public string FisTuru { get; set; }
        public string FisSeri { get; set; }
        public int FisNo { get; set; }
        public DateTime? Tarih { get; set; }
        public string Personel { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public string Degistiren { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        //public string CariKodu { get; set; }
        //public string CariAdi { get; set; }
        //public string IrsEvrakSeri { get; set; }
        //public string IrsEvrakSira { get; set; }
        public Guid? DkId { get; set; }
        public Guid? IrsGuid { get; set; }
        //public string IrsBelgeNo { get; set; }
        public DepoStokFis() {
            Id = Guid.Empty;
            FisSeri = "";
            FisNo = 0;
            Tarih = DateTime.Now;
            Aciklama = "";
            Personel = "";
            DkId = Guid.Empty;
            IrsGuid = Guid.Empty;
        }

        public static string GetSelectSqlCode(string sor = "") {
            string sql = @"SELECT *  FROM DepoStokFis  WITH (NOLOCK) " + sor;
            return sql;
        }
        public static string GetSelectSqlCodeById(Guid? fisId) {
            string sql = $@"SELECT *  FROM DepoStokFis  WITH (NOLOCK) where  Id='{fisId}'";
            return sql;
        }
        public static string GetInsertSqlCode() {
            string sql = @"  IF EXISTS
  (SELECT * FROM  DepoStokFis  WHERE Id = @Id )
    UPDATE  DepoStokFis SET
    FisTuru            =@FisTuru            ,
    FisSeri            =@FisSeri            ,
    FisNo              =@FisNo              ,
    Tarih              =@Tarih              ,
    Personel           =@Personel           ,
    Aciklama           =@Aciklama           ,
    KayitEden          =@KayitEden          ,
    Degistiren         =@Degistiren         ,
    KayitTarihi        =@KayitTarihi        ,
    DegistirmeTarihi   =@DegistirmeTarihi     
  
    WHERE Id = @Id
  ELSE
    INSERT INTO  DepoStokFis  (
Id                     ,
FisTuru                ,
FisSeri                ,
FisNo                  ,
Tarih                  ,
Personel               ,
Aciklama               ,
KayitEden              ,
Degistiren             ,
KayitTarihi            ,
DegistirmeTarihi       , 
DkId                   ,
IrsGuid                
     )
    VALUES(
@Id                     ,
@FisTuru                ,
@FisSeri                ,
@FisNo                  ,
@Tarih                  ,
@Personel               ,
@Aciklama               ,
@KayitEden              ,
@Degistiren             ,
@KayitTarihi            ,
@DegistirmeTarihi       , 
@DkId                   ,
@IrsGuid                   
      );";
            return sql;
        }

    }
}
