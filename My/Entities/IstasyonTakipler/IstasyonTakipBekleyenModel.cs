using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.IstasyonTakipler {
    public class IstasyonTakipBekleyenModel {

        public Guid UrId { get; set; }
        public Guid UrIId { get; set; }
        public string SiparisKodu { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double KalanMiktar { get; set; }
        public double PlanlananMiktar { get; set; }
        public double UretimMiktari { get; set; }
        public double FireMiktari { get; set; }
        public double IptalMiktari { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public bool Fason { get; set; }
        public string FasonCariKodu { get; set; }
        public string FasonCariUnvani { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public string TalepEden { get; set; }
        public string Aciklama { get; set; }


    }

}
