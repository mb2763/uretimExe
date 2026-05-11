using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Personeller
{
    [Table("Personel")]
    public class Personel
    {
        public Personel()
        {
            Id = MyGuid.NewGuid();
            Kodu = "";
            Adi = "";
            Soyadi = "";
            Grup = "";
            Gorevi = "";
        } 
        [Key] public Guid? Id { get; set; } 
        public string Kodu { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Grup { get; set; }
        public string Gorevi { get; set; }
        public bool Yetkili { get; set; } 
        public bool Admin { get; set; }
        public string Sifre { get; set; }
        public bool IstasyonPersoneli { get; set; }    
        public string IstasyonKodu { get; set; }   
        public string CepTel { get; set; }
        public bool SmsGonder { get; set; }


        [ComVisible(true)]
        public Personel Clone()
        {
            return (Personel) MemberwiseClone();
        }

        public static string GetInsOrUpdCode() { 
            string sql = @"  IF EXISTS
     (SELECT * FROM dbo.Personel WHERE Id = @Id)
      UPDATE dbo.Personel SET
      Kodu = @Kodu,
      Adi = @Adi   ,
      Soyadi = @Soyadi  , 
      Grup = @Grup  , 
      Gorevi=@Gorevi,
      Yetkili=@Yetkili,
      Admin=@Admin,
      Sifre=@Sifre,
      IstasyonPersoneli=@IstasyonPersoneli,
      IstasyonKodu=@IstasyonKodu,
      CepTel=@CepTel,
      SmsGonder=@SmsGonder
      WHERE Id = @Id
     ELSE
      INSERT INTO dbo.Personel (
        Id,  Kodu,  Adi,  Soyadi,  Grup ,  Gorevi ,  Yetkili, Admin, Sifre, IstasyonPersoneli, IstasyonKodu,CepTel,SmsGonder)
     VALUES(
       @Id, @Kodu, @Adi, @Soyadi, @Grup , @Gorevi , @Yetkili,@Admin,@Sifre,@IstasyonPersoneli,@IstasyonKodu,@CepTel,@SmsGonder );";

            return sql;
        }
        public static string GetUpdCodeSifresiz() {

       string sql = @"   
      UPDATE dbo.Personel SET
      Kodu = @Kodu,
      Adi = @Adi   ,
      Soyadi = @Soyadi  , 
      Grup = @Grup  , 
      Gorevi=@Gorevi,
      Yetkili=@Yetkili,
      Admin=@Admin,
      IstasyonPersoneli=@IstasyonPersoneli,
      IstasyonKodu=@IstasyonKodu,
      CepTel=@CepTel,
      SmsGonder=@SmsGonder
      WHERE Id = @Id  ;";

            return sql;
        }
         

    }
}