using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.UretimEmirler
{
    [Table("UretimEmri")]
    public class UretimEmri
    {
        public UretimEmri()
        {
            Id = MyGuid.NewGuid();
        } 
        [Key] public Guid? Id { get; set; }
        public string Turu { get; set; }
        public string Durumu { get; set; }
        public string IsEmriNo { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string SiparisKodu { get; set; }
        public string SiparisCariKodu { get; set; }
        public string SiparisCariUnvani { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string Aciklama { get; set; }
        public string IstasyonGrupKodu { get; set; } 

        // public double PlanlananMiktar { get; set; }
        // public double UretimMiktari { get; set; }
        // public double FireMiktari { get; set; }
        // public double SiparisMiktar { get; set; }

        public double HesaplananMaliyet { get; set; }
        public double StokMaliyet { get; set; }
        public double OperasyonMaliyetFiyat { get; set; }
        public double OperasyonMaliyet { get; set; }
        public double IstasyonMaliyetFiyat { get; set; }
        public double IstasyonMaliyet { get; set; }
        public double NetMaliyet { get; set; }
        public bool Kapandi { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }

        public Guid? RcAId { get; set; }
        public Guid? SipId { get; set; }  

        [ComVisible(true)]
        public UretimEmri Clone()
        {
            return (UretimEmri) MemberwiseClone();
        }
    }
}