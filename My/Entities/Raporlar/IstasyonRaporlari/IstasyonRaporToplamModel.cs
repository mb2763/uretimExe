namespace My.Entities.Raporlar.IstasyonRaporlari {
    public class IstasyonRaporToplamModel { 
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double UretimMiktar { get; set; }
        public double FireMiktar { get; set; }
        public double IptalMiktar { get; set; }
 
        public static string GetSelectSqlCode(string and_sorgu) {
            string sql = @"   
               SELECT   HR.StokKodu, HR.StokAdi,  
SUM(COALESCE( HR.Miktar,0)) AS UretimMiktar,
SUM(COALESCE( HR.FireMiktar,0)) AS FireMiktar, 
SUM(COALESCE(HR.IptalMiktar,0)) AS IptalMiktar  
FROM IstasyonTakipHareketDetay HR 
LEFT OUTER JOIN IstasyonTakipHareket IST ON HR.IstHrId = IST.Id 
WHERE HR.Turu IN ('MamulGiris','FireMamulGiris','UretimBitis','UretimIptal') " + and_sorgu + @"
GROUP BY  HR.StokKodu, HR.StokAdi 
ORDER BY Hr.StokKodu ";

            return sql;
        }
    }
}
