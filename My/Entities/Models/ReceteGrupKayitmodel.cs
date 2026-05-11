using My.Entities.ReceteGruplar;
using System.Collections.Generic;

namespace My.Entities.Models
{
    public class ReceteGrupKayitmodel
    {
        public ReceteGrupKayitmodel()
        {
            Grup = new ReceteGrup();
            Detaylar = new List<ReceteGrupDetay>();
        }

        public ReceteGrup Grup { get; set; }
        public List<ReceteGrupDetay> Detaylar { get; set; }
    }
}