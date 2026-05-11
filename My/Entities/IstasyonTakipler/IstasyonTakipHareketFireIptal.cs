using System; 

namespace My.Entities.IstasyonTakipler {
    public class IstasyonTakipHareketFireIptal {
        public  IstasyonTakipHareketFireIptal Clone()
        {
                return (IstasyonTakipHareketFireIptal)MemberwiseClone();
        }
        //public string Turu { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        //public double KullanilanMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        //public string Birim { get; set; }
        //public string Renk { get; set; }
        //public string Beden { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }

    }
}
