using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.UretimOperasyonlar
{
    [Table("UretimOperasyon")]
    public class UretimOperasyon
    {
        public UretimOperasyon()
        {
            Id = MyGuid.NewGuid();
        }

        [Key] public Guid? Id { get; set; }
        public string IsEmriNo { get; set; }
        public string Durumu { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public double KalanMiktar { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public int Sira { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public Guid? UrId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }
        public double KullanilanAparat { get; set; }

        [ComVisible(true)]
        public UretimOperasyon Clone()
        {
            return (UretimOperasyon) MemberwiseClone();
        }
        public static string GetSelectSqlCode(string whereSql) {
            var sql = @"  select UrO.* from UretimOperasyon  UrO left outer join Siparis Sip on Sip.Id =UrO.SipId   " + whereSql + "; ";
            return sql;
        }
    }
}