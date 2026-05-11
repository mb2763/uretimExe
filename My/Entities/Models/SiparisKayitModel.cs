using My.Entities.Siparisler;
using System.Collections.Generic;

namespace My.Entities.Models
{
    public class SiparisKayitModel
    {
        public SiparisKayitModel()
        {
            Siparis = new Siparis();
            Hareketler = new List<SiparisHareket>();
            Detaylar = new List<SiparisHareketDetay>();
        }

        public Siparis Siparis { get; set; }
        public List<SiparisHareket> Hareketler { get; set; }
        public List<SiparisHareketDetay> Detaylar { get; set; }
    }
}