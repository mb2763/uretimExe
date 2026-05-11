using My.Core;
using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.UretimIstasyonlar
{
    [Table("UretimIstasyon")]
    public class UretimIstasyon {
        public UretimIstasyon() {
            Id = MyGuid.NewGuid();
        }

        [Key] public Guid? Id { get; set; }
        [Ignore] public string ReceteKodu { get; set; }
        [Ignore] public string ReceteAdi { get; set; }
        [Ignore] public string OperasyonKodu { get; set; }
        [Ignore] public string OperasyonAdi { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string IstDurumu { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        [Ignore] public double KalanMiktar { get; set; }
        public DateTime? BaslangicTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public bool Fason { get; set; }
        public string FasonCariKodu { get; set; }
        public string FasonCariUnvani { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrOId { get; set; }
        public Guid? UrOHId { get; set; }
        public Guid? UrOHDId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? RcIstId { get; set; }
        public Guid? SipId { get; set; }
        

        [ComVisible(true)]
        public UretimIstasyon Clone() {
            return (UretimIstasyon)MemberwiseClone();
        }

        public static string GetSelectSqlCode(string whereSql) {

            var sql = @"  select UrI.*,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi,
                             coalesce( UrI.PlanlananMiktar,0) -  (coalesce( UrI.UretimMiktari,0) + coalesce( UrI.IptalMiktari,0)) as KalanMiktar
                             from UretimIstasyon UrI 
							 left outer join UretimOperasyon UrO on UrO.Id = UrI.UrOId
							 left outer join UretimOperasyonHareket UrOH on UrOH.Id = UrI.UrOHId  " + whereSql + "; ";

            return sql;
        }

    }
}