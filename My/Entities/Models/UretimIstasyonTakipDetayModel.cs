using System;
using System.Runtime.InteropServices;

namespace My.Entities.Models
{
    public class UretimIstasyonTakipDetayModel
    {
        public string IsEmriKodu { get; set; }
        public string IsEmriNo { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string OperasyonKodu { get; set; } 
        public string ReceteAdi { get; set; } 
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public string OperasyonAdi { get; set; }
        public string ReceteKodu { get; set; }
        public bool Fason { get; set; }
        public string FasonCariKodu { get; set; }
        public string FasonCariUnvani { get; set; }
        [ComVisible(true)]
        public UretimIstasyonTakipDetayModel Clone()
        {
            return (UretimIstasyonTakipDetayModel) MemberwiseClone();
        }
    }
}