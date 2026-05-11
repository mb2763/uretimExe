using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.UretimOperasyonlar
{
    [Table("UretimOperasyonHareketDetay")]
    public class UretimOperasyonHareketDetay
    {
        public UretimOperasyonHareketDetay()
        {
            Id = MyGuid.NewGuid();
        }

        [Key] public Guid? Id { get; set; }
        [Ignore] public string IsEmriNo { get; set; }
        [Ignore] public string ReceteKodu { get; set; }
        [Ignore] public string ReceteAdi { get; set; }
        [Ignore] public string OperasyonKodu { get; set; }
        [Ignore] public string OperasyonAdi { get; set; }
        public string Turu { get; set; }

        public DateTime? Tarih { get; set; }
        public double PlanlananMiktar { get; set; }
        public double IslemdekiMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }

        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrOId { get; set; }
        public Guid? UrOHId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? SipId { get; set; }

        [ComVisible(true)]
        public UretimOperasyonHareketDetay Clone()
        {
            return (UretimOperasyonHareketDetay) MemberwiseClone();
        }
        public static string GetSelectSqlCode(string whereSql) {
            var sql =
               @"    select UrOHD.*,UrO.IsEmriNo,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi     
                 from UretimOperasyonHareketDetay UrOHD  
                 left outer join UretimOperasyonHareket UrOH on UrOH.Id = UrOHD.UrOHId 
                 left outer join UretimOperasyon        UrO  on UrO.Id = UrOHD.UrOId  " + whereSql + "; ";
            return sql;
        }
    }
}