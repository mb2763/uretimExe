using System;

namespace My.Entities.Models
{
    public class MikroReceteHareketModel
    {
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string AnaBirimi { get; set; }
        public double AnaMiktar { get; set; }
        public string Turu { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public string Birimi { get; set; }
        public int Sira { get; set; }
        public string Depo { get; set; }
        public DateTime? ReceteTarihi { get; set; }
        public Guid? ReceteGuid { get; set; }

        public double FireYuzde { get; set; }

    }
}