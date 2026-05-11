using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.UretimTalepler
{

    [Table("UretimTalepHareket")]
    public class UretimTalepHareket
    {

        [Key]
        public Guid UrtTlpHrId { get; set; }
        public Guid UrtTlpId { get; set; }
        public DateTime? Tarih { get; set; }
        public string EvrakNo { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string Kullanici { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public string Birimi { get; set; }
        public string Aciklama { get; set; }
        public string Parti { get; set; }
        public int Lot { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public short Ent { get; set; }
        public Guid? EntId { get; set; }
        public string EntKodu { get; set; }
        public DateTime? EntTarih { get; set; }

        [ComVisible(true)]
        public UretimTalepHareket Clone()
        {
            return (UretimTalepHareket)MemberwiseClone();
        }

    }
}
