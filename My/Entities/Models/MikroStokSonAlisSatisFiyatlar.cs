 

namespace My.Entities.Models {
    public class MikroStokSonAlisSatisFiyatlar {
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double SonAlis { get; set; }
        public double Son5AlisOrtalama  { get; set; }
        public double StandartMaliyet { get; set; }
        public double SonSayimGiris { get; set; }
        public double DevirGiris { get; set; }
    }
}
