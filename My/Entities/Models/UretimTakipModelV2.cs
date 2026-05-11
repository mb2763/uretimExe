using System.Runtime.InteropServices;

namespace My.Entities.Models {
    public class UretimTakipModelV2 {
        public string Turu { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string ReceteAdi { get; set; }
        public string SiparisKodu { get; set; }
        public string CariUnvani { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double IslemdekiMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double KalanMiktar { get; set; }

        [ComVisible(true)]
        public UretimTakipModelV2 Clone() {
            return (UretimTakipModelV2)MemberwiseClone();
        }
    }
}