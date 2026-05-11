using System.Collections.Generic;

namespace My.Entities.Raporlar.IstasyonRaporlari {
    public class IstasyonRaporModel {

        public List<IstasyonRaporHareketModel> Hareketler { get; set; }
        public List<IstasyonRaporToplamModel> Toplamlar { get; set; }

        public IstasyonRaporModel() {
            Hareketler = new List<IstasyonRaporHareketModel>();
            Toplamlar = new List<IstasyonRaporToplamModel>();
        }

    }
}
