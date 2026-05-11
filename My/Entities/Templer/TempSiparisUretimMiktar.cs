using My.Core;
using My.Core.Data;
using System;

namespace My.Entities.Templer {

    [Table("TempSiparisUretimMiktar")]
    public class TempSiparisUretimMiktar {

        public TempSiparisUretimMiktar() {
            Id = MyGuid.NewGuid();
        }
        [Key] public Guid? Id { get; set; }
        public string Turu { get; set; }
        public string Kullanici { get; set; }
        public string IsEmriKodu { get; set; }
        public string IsEmriNo { get; set; }
        public int Sira { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrIId { get; set; }
        public Guid? UrOId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }

    }
}
