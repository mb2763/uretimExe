using My.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.IstasyonTakipler {
    
    [Table("IstasyonTakipStokHareketDetay")]
    public class IstasyonTakipStokHareketDetayDetayli {
        //[Key] public Guid? Id { get; set; }
    
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
        public double StokMiktar { get; set; }
        public double StokFireMiktar { get; set; }
        public double StokIptalMiktar { get; set; }   
        public string IsEmriKodu { get; set; }
        public string IsEmriNo { get; set; }
        public string UrtStokKodu { get; set; }
        public string UrtStokAdi { get; set; }
        public double UretimMiktar { get; set; }
        public double FireMiktar { get; set; }
        public double IptalMiktar { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }

        //public string Parti { get; set; }
        //public string Lot { get; set; }
        //public double SipAdet { get; set; }
        //public double Carpan { get; set; } 
        //public string Turu { get; set; }

        [Ignore] public string KategoriKodu { get; set; }
        [Ignore] public string KategoriAdi { get; set; }
        [Ignore] public string KaliteKontrolKodu { get; set; }
        [Ignore] public string KaliteKontrolAdi { get; set; }
        [Ignore] public string ReyonKodu { get; set; }
        [Ignore] public string ReyonAdi { get; set; }
        //[Ignore] public DateTime? Tarih { get; set; }

    }
}
