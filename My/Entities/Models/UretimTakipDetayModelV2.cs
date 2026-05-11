using System;
using System.Runtime.InteropServices;

namespace My.Entities.Models
{
    public class UretimTakipDetayModelV2
    {
        public Guid? Id { get; set; }
        public string Durumu { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double IslemdekiMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public double KalanMiktar { get; set; }
        public string SiparisKodu { get; set; }
        public string CariKodu { get; set; }
        public string CariUnvani { get; set; }

        [ComVisible(true)]
        public UretimTakipDetayModelV2 Clone()
        {
            return (UretimTakipDetayModelV2) MemberwiseClone();
        }
    }
}