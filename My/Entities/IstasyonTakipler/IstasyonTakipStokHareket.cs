using My.Core.Data;
using System;

namespace My.Entities.IstasyonTakipler {

    [Table("IstasyonTakipStokHareket")]
    public class IstasyonTakipStokHareket {

        public Guid? Id { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; } 
        public double PlanlananMiktar { get; set; }
        public double KullanilanMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public string Aciklama { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrIId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }
        public Guid? UrSTId { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; } 
        public bool Ent { get; set; }
        public string EntCode { get; set; }
        public DateTime? EntDate { get; set; }
        public string EntSeri { get; set; }
        public string EntSira { get; set; }
        public static string GetSelectSqlCode(string sorgu) {
            string sql = @" SELECT ISTS.*   FROM IstasyonTakipStokHareket ISTS 
  left outer join UretimIstasyon UrI ON ISTS.UrIId = UrI.Id  
 left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
 left outer join Siparis sip  on UrO.SipId = sip.Id
 left outer join SiparisHareket siph  on UrO.SipHId = siph.Id   " + sorgu;
            return sql;
        }
        public static string GetSelectSqlCodeById(Guid id) {
            string sql = @" SELECT ISTS.*   FROM IstasyonTakipStokHareket ISTS 
  left outer join UretimIstasyon UrI ON ISTS.UrIId = UrI.Id  
 left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
 left outer join Siparis sip  on UrO.SipId = sip.Id
 left outer join SiparisHareket siph  on UrO.SipHId = siph.Id  where ISTS.Id='" + id + "'";
            return sql;
        }

        public static string GetStokHareketByUrIId(Guid urIId) {
            string sql = @" SELECT ISTS.*   FROM IstasyonTakipStokHareket ISTS 
  left outer join UretimIstasyon UrI ON ISTS.UrIId = UrI.Id  
 left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
 left outer join Siparis sip  on UrO.SipId = sip.Id
 left outer join SiparisHareket siph  on UrO.SipHId = siph.Id  where ISTS.UrIId='" + urIId + "'";
            return sql;
        }
         
    }
}
