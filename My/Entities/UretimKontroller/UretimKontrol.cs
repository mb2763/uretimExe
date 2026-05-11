using My.Core.Data;
using My.Core;
using System;

namespace My.Entities.UretimKontroller
{
    [Table("UretimKontrol")]
    public class UretimKontrol
    {
        public UretimKontrol()
        {
            Id = MyGuid.NewGuid();
        }
        [Key] public Guid? Id { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrIId { get; set; }
        public Guid? IstHrId { get; set; }
        public string Turu { get; set; }
        public DateTime? Tarih { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double OlcumDegeri { get; set; }
        public double OlcumDegeri2 { get; set; }
        public double OlcumDegeri3 { get; set; }

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
        public bool HataliGiris { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }


        [Ignore] public string IsEmriNo { get; set; }
        [Ignore] public string Kullanici { get; set; }
        [Ignore] public string IsEmriKodu { get; set; }

    }
}
