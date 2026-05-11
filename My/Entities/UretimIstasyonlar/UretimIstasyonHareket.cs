using System;
using System.Runtime.InteropServices;
using My.Core.Data;

namespace My.Entities.UretimIstasyonlar
{
    [Table("UretimIstasyonHareket")]
    public class UretimIstasyonHareket
    {
        [Key] public Guid? Id { get; set; }
        [Ignore] public string ReceteKodu { get; set; }
        [Ignore] public string ReceteAdi { get; set; }
        [Ignore] public string OperasyonKodu { get; set; }
        [Ignore] public string OperasyonAdi { get; set; }

        public DateTime? Tarih { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        [Ignore] public bool Fason { get; set; }
        [Ignore] public string FasonCariKodu { get; set; }
        [Ignore] public string FasonCariUnvani { get; set; }

        public string PersonelKodu { get; set; }
        public string PersonelAdi { get; set; }

        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrOId { get; set; }
        public Guid? UrOHId { get; set; }
        public Guid? UrOHDId { get; set; }
        public Guid? UrIId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? SipId { get; set; }

        [ComVisible(true)]
        public UretimIstasyonHareket Clone()
        {
            return (UretimIstasyonHareket) MemberwiseClone();
        }


        public static string GetSelectSqlCode(string whereSql) {

            var sql =
               @"  select UrIH.*,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi  ,UrI.Fason,UrI. FasonCariKodu, UrI. FasonCariUnvani 
                             from UretimIstasyonHareket UrIH 
							 left outer join UretimIstasyon UrI  on UrI.Id = UrIH.UrIId 
							 left outer join UretimOperasyon UrO on UrO.Id =UrIH.UrOId 
							 left outer join UretimOperasyonHareket UrOH on UrOH.Id = UrIH.UrOHId   " + whereSql +
               "; ";

            return sql;
        }

    }
}