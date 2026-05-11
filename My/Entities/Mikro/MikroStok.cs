using System;
using System.Runtime.InteropServices;

namespace My.Entities.Mikro
{
    public class MikroStok : IEntity
    {
        public Guid StGuid { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; } 
        public string AnaGrup { get; set; }

        //    public string AltGrup { get; set; }
        public string Dvz { get; set; }
        public string Birim { get; set; }

        public string RenkKodu { get; set; }
        public string BedenKodu { get; set; }
        public string Fiyati { get; set; }
        public string StokCinsi { get; set; }
        public string ModelKodu { get; set; }

        [ComVisible(true)]
        public MikroStok Clone()
        {
            return (MikroStok) MemberwiseClone();
        }
    }
}