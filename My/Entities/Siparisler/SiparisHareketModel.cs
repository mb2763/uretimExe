using System;

namespace My.Entities.Siparisler
{
    public class SiparisHareketModel
    {
        public Guid Id { get; set; } 
        public string SiparisKodu { get; set; } 
        public string CariKodu { get; set; } 
        public string CariUnvani { get; set; } 
        public DateTime? Tarih { get; set; } 
        public DateTime? TeslimTarihi { get; set; }
        public string ReceteGrupKodu { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public double KalanMiktar { get; set; }
        public string Renk { get; set; }
        public string Birim { get; set; }
        public string Aciklama { get; set; }
        public Guid? SipId { get; set; }
        public Guid? RcAId { get; set; }

        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }


    }
}