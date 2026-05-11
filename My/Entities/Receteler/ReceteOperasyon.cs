using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Receteler
{
    [Table("ReceteOperasyon")]
    public class ReceteOperasyon
    {
        public ReceteOperasyon()
        {
            Id = MyGuid.NewGuid();
            OperasyonKodu = "";
            OperasyonAdi = "";
            Sira = 0;
            UretimSure = 0;
            KullanilacakAparat = "";
            OlcumDegeriMin = 0;
            OlcumDegeriMax = 0;
            OlcumDegeriMin2 = 0;
            OlcumDegeriMax2 = 0;
            OlcumDegeriMin3 = 0;
            OlcumDegeriMax3 = 0;
            RcAId = Guid.Empty; 
        }

        [Key] public Guid? Id { get; set; } 
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public double MaliyetFiyat { get; set; }
        public int Sira { get; set; }
        public double UretimSure { get; set; }
        public string KullanilacakAparat { get; set; }
        public double OlcumDegeriMin { get; set; }
        public double OlcumDegeriMax { get; set; }
        public double OlcumDegeriMin2 { get; set; }
        public double OlcumDegeriMax2 { get; set; }
        public double OlcumDegeriMin3 { get; set; }
        public double OlcumDegeriMax3 { get; set; }
        public double OlcumDegeriMin4 { get; set; }
        public double OlcumDegeriMax4 { get; set; }
        public double OlcumDegeriMin5 { get; set; }
        public double OlcumDegeriMax5 { get; set; }
        public Guid? RcAId { get; set; }

        [ComVisible(true)]
        public ReceteOperasyon Clone()
        {
            return (ReceteOperasyon) MemberwiseClone();
        }

        public void SetDetay(ReceteOperasyon dty)
        {
            Id = dty.Id;
            OperasyonKodu = dty.OperasyonKodu;
            OperasyonAdi = dty.OperasyonAdi;
            Sira = dty.Sira;
            UretimSure = dty.Sira;
            RcAId = dty.RcAId;            
        }
    }
}