using System; 

namespace My.Entities.Raporlar.IstasyonRaporlari {
    public class IstasyonRaporHareketModel { 
        public DateTime? Tarih { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string IstasyonKodu { get; set; }
        public string Personel { get; set; }
        public string PersonelAdi { get; set; }
        public double UretimMiktar { get; set; }
        public double FireMiktar { get; set; }
        public double IptalMiktar { get; set; }

        public static string GetSelectSqlCode(string and_sorgu) {
            string sql = @"   
              SELECT  CAST(HR.Tarih AS DATE) AS Tarih, HR.StokKodu, HR.StokAdi, 
IST.IstasyonKodu,
coalesce(HR.KayitEden,'') AS Personel , 
Trim(coalesce(Prs.Adi,'') + ' ' + coalesce(Prs.Soyadi,'')) AS PersonelAdi ,
SUM(COALESCE( HR.Miktar,0)) AS UretimMiktar,
SUM(COALESCE( HR.FireMiktar,0)) AS FireMiktar, 
SUM(COALESCE(HR.IptalMiktar,0)) AS IptalMiktar  
FROM IstasyonTakipHareketDetay HR
LEFT OUTER JOIN IstasyonTakipHareket IST ON HR.IstHrId = IST.Id 
LEFT OUTER JOIN Personel Prs ON Prs.Kodu = Hr.KayitEden
WHERE HR.Turu IN ('MamulGiris','FireMamulGiris','UretimBitis','UretimIptal')" + and_sorgu + @"
GROUP BY CAST(HR.Tarih AS DATE), HR.StokKodu, HR.StokAdi,  coalesce(HR.KayitEden,''),IST.IstasyonKodu,
Trim(coalesce(Prs.Adi,'') + ' ' + coalesce(Prs.Soyadi,''))
ORDER BY Hr.StokKodu ";

            return sql;
        }
    }
}
