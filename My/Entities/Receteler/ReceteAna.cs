using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Receteler
{
    [Table("ReceteAna")]
    public class ReceteAna
    {
        public ReceteAna()
        {
            Id = MyGuid.NewGuid();
            StokCinsiKodu = -1;
        }

        [Key] public Guid? Id { get; set; } 
        public string Grubu { get; set; } 
        public string ReceteKodu { get; set; } 
        public string ReceteAdi { get; set; } 
        public string Aciklama { get; set; }
        public string AmbalajSekli { get; set; }
        public string EntegreStokKodu { get; set; }
        public string EntegreStokAdi { get; set; }
        public string EntegreBirim { get; set; }
        public string ModelKodu { get; set; }
        public int StokCinsiKodu { get; set; }
        public string StokCinsiAdi { get; set; } 
        public string KayitEden { get; set; }
        public string Degistiren { get; set; } 
        public DateTime? KayitTarihi { get; set; }
        public DateTime? DegistirmeTarihi { get; set; } 
        public bool HaziriSonrakiIstasyonaGonder { get; set; }
        public bool IstasyonGruplamaKullan { get; set; }
        public bool AparatZorunlu { get; set; }
        public bool OlcumZorunlu { get; set; }
        public int RafOmru { get; set; }
        [Ignore]
        public int OperasyonAdet { get; set; }
        [Ignore]
        public int IstasyonAdet { get; set; }    

        [ComVisible(true)]
        public ReceteAna Clone()
        {
            return (ReceteAna) MemberwiseClone();
        }
    }
}