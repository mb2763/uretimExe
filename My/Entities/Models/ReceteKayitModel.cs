using My.Entities.ReceteIstasyonlar;
using My.Entities.Receteler;
using My.Entities.ReceteStoklar;
using System.Collections.Generic;

namespace My.Entities.Models
{
    public class ReceteKayitModel
    {
        public ReceteKayitModel Clone()
        {
            return (ReceteKayitModel)MemberwiseClone();
        }

        public ReceteKayitModel()
        {
            Recete = new ReceteAna();
            ReceteDetaylar = new List<ReceteDetay>();
            ReceteStoklar = new List<ReceteStok>();
            ReceteyeBagliIstasyonlar = new List<ReceteyeBagliIstasyon>();
            ReceteStokRenkBedenler = new List<ReceteStokRenkBeden>();
        }

        public ReceteAna Recete { get; set; }

        public List<ReceteDetay> ReceteDetaylar { get; set; }
        public List<ReceteStok> ReceteStoklar { get; set; }
        public List<ReceteStokRenkBeden> ReceteStokRenkBedenler { get; set; }
        public List<ReceteyeBagliIstasyon> ReceteyeBagliIstasyonlar { get; set; }
    }
}