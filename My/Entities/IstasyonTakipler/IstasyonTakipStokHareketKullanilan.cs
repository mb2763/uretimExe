using System; 

namespace My.Entities.IstasyonTakipler {
    public class IstasyonTakipStokHareketKullanilan {
        public string StokKodu { get; set; }
        public string StokAdi { get; set; } 
        public double SipAdet { get; set; }
        public double Carpan { get; set; } // PlanlananMiktar / UretimMiktari
        public double StokMiktar { get; set; }
        public double StokFireMiktar { get; set; }
        public double StokIptalMiktar { get; set; } 
        public double UretimMiktari { get; set; }
        public double UretimFireMiktari { get; set; }
        public double UretimIptalMiktari { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }



        // public double PlanlananMiktar { get; set; } 
        // public double KullanilanMiktar { get; set; }
        // public double IptalMiktari { get; set; } 
        // public double FireMiktari { get; set; } 

        public IstasyonTakipStokHareketKullanilan Clone()
        {
            return (IstasyonTakipStokHareketKullanilan)MemberwiseClone();
        }
    }
}
