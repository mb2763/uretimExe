using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Siparisler
{
    [Table("SiparisHareketDetay")]
    public class SiparisHareketDetay
    {
        public SiparisHareketDetay()
        {
            Id = MyGuid.NewGuid();
            Cinsi = "";
            StokKodu = "";
            StokAdi = "";
            Birim = "";
            Renk = "";
            Beden = "";
            Miktar = 0;
            Aciklama = "";
            SipId = Guid.Empty;
            SipHId= Guid.Empty; 
            RcAId = Guid.Empty;
            RcDId = Guid.Empty;
        }

        [Key] public Guid? Id { get; set; } 
        public string Cinsi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public double Miktar { get; set; }
        public string Aciklama { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; } 
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }

        [ComVisible(true)]
        public SiparisHareketDetay Clone()
        {
            return (SiparisHareketDetay) MemberwiseClone();
        }

        public void SetDetay(SiparisHareketDetay dty)
        {
            Id = dty.Id;
            Cinsi = dty.Cinsi;
            StokKodu = dty.StokKodu;
            StokAdi = dty.StokAdi;
            Birim = dty.Birim;
            Renk = dty.Renk;
            Beden = dty.Beden;
            Miktar = dty.Miktar;
            Aciklama = dty.Aciklama;
            SipId = dty.SipId;
            SipHId = dty.SipHId; 
            RcAId = dty.RcAId;
            RcDId = dty.RcDId;
        }
    }
}