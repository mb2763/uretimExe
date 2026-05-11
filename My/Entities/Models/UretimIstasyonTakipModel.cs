using System.Runtime.InteropServices;

namespace My.Entities.Models {
    public class UretimIstasyonTakipModel {
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public bool Fason { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; } 
        public string FasonCariKodu { get; set; }
        public string FasonCariUnvani { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }

        [ComVisible(true)]
        public UretimIstasyonTakipModel Clone() {
            return (UretimIstasyonTakipModel)MemberwiseClone();
        }
    }
}