using My.Core.Data;
using System;

namespace My.Entities.IstasyonTakipler {
    [Table("IstasyonTakipHareketLog")]
    public class IstasyonTakipHareketLog {
        [Key] public Guid? Id { get; set; }
        public Guid? IstHrId { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrIId { get; set; }
        public DateTime? Tarih { get; set; }
        public string Turu { get; set; }
        public string Kodu { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }


        [Ignore] public string ReceteKodu { get; set; }
        [Ignore] public string ReceteAdi { get; set; }
        [Ignore] public string OperasyonKodu { get; set; }
        [Ignore] public string OperasyonAdi { get; set; }
        [Ignore] public string IstasyonKodu { get; set; }
        [Ignore] public string IstasyonAdi { get; set; }

        [Ignore] public string IsEmriNo { get; set; }
        [Ignore] public string Kullanici { get; set; }
        [Ignore] public string IsEmriKodu { get; set; }
        [Ignore] public double PlanlananMiktar { get; set; }
        [Ignore] public double UretimMiktari { get; set; }
        [Ignore] public string Durumu { get; set; }


    }

}
