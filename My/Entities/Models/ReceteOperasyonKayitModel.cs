using My.Entities.ReceteIstasyonlar;
using My.Entities.Receteler;
using System.Collections.Generic;

namespace My.Entities.Models
{
    public class ReceteOperasyonKayitModel
    {
        public ReceteOperasyonKayitModel()
        {
            Recete = new ReceteAna();
            Operasyonlar = new List<ReceteOperasyon>();
            Istasyonlar = new List<ReceteIstasyon>();
            Cariler = new List<ReceteIstasyonCari>();
        }

        public ReceteAna Recete { get; set; }
        public List<ReceteOperasyon> Operasyonlar { get; set; }
        public List<ReceteIstasyon> Istasyonlar { get; set; }
        public List<ReceteIstasyonCari> Cariler { get; set; }
    }
}