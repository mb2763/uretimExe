using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.IstasyonBakimlar
{
    [Table("IstasyonBakim")]
    public class IstasyonBakim {

        [Key] public Guid? Id { get; set; }
        public string IstasyonKodu { get; set; }
        public DateTime? Tarih { get; set; }
        public string Personel { get; set; }
        public string IslemTuru { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public string Degistiren { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }

        [ComVisible(true)]
        public IstasyonBakim Clone() {
            return (IstasyonBakim)MemberwiseClone();
        }
    }
}
