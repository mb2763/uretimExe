using System;

namespace My.Entities.Models
{
    public class OperasyonKalanKontrolModel
    {
        public Guid? UrId { get; set; }
        public Guid? UrOId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public int Sira { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double IslemdekiMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public double KalanMiktar { get; set; }
    }
}