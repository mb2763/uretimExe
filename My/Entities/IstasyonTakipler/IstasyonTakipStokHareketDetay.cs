using My.Core.Data;
using System;

namespace My.Entities.IstasyonTakipler {
    [Table("IstasyonTakipStokHareketDetay")]
    public class IstasyonTakipStokHareketDetay {
        [Key] public Guid? Id { get; set; }
        public string IsEmriKodu { get; set; }
        public string UrtStokKodu { get; set; }
        public string UrtStokAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Turu { get; set; }
        public double SipAdet { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public double UretimMiktar { get; set; }
        public double FireMiktar { get; set; }
        public double IptalMiktar { get; set; }
        public double Carpan { get; set; }
        public double StokMiktar { get; set; }
        public double StokFireMiktar { get; set; }
        public double StokIptalMiktar { get; set; }
        public Guid UrId { get; set; }
        public Guid UrIId { get; set; }
        public Guid UrSTId { get; set; }
        public Guid IstHrId { get; set; }
        public Guid SipId { get; set; }
        public Guid SipHId { get; set; }
     

    }
}
