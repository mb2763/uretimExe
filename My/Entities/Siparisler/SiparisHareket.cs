using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Siparisler
{
    [Table("SiparisHareket")]
    public class SiparisHareket
    {
        public SiparisHareket()
        {
            Id =MyGuid.NewGuid();
            SipId = Guid.Empty;
            RcAId = Guid.Empty; 
            YeniKayit = false;
        } 
        [Key] public Guid? Id { get; set; } 
        public string ReceteGrupKodu { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }

        [Ignore]
        public double KalanMiktar { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Birim { get; set; }
        public string Aciklama { get; set; }
        public string EtiketAciklama { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public Guid? SipId { get; set; }
        public Guid? RcAId { get; set; }
        public bool Ent { get; set; }
        public string EntCode { get; set; }
        public DateTime? EntDate { get; set; }
        public string EntSeri { get; set; }
        public string EntSira { get; set; } 
        public string EntKayitSeri { get; set; }
        public string EntKayitSira { get; set; }
        public Guid? EntKayitGuid { get; set; } 
        public bool EtiketBasildi { get; set; } 
        [Ignore] public bool YeniKayit { get; set; }
        [Ignore] public DateTime? Tarih { get; set; }

        [ComVisible(true)]
        public SiparisHareket Clone()
        {
            return (SiparisHareket) MemberwiseClone();
        }
    }
}