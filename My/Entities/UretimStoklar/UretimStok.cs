using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.UretimStoklar
{
    [Table("UretimStok")]
    public class UretimStok
    {
        public UretimStok()
        {
            Id = MyGuid.NewGuid();
        }
        [Key] public Guid? Id { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public double Miktar { get; set; }
        public double PlanlananMiktar { get; set; }
        public double KullanilanMiktar { get; set; }
        public double FireMiktari { get; set; }
        public double Fiyat { get; set; }
        public double Tutar { get; set; }
        public Guid? UrId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }

        [ComVisible(true)]
        public UretimStok Clone()
        {
            return (UretimStok)MemberwiseClone();
        }
    }
}