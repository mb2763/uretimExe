using My.Entities.IstasyonTakipler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Models {
    public class MalKabulFisKullanilanStokModel {
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
     
        public double Miktar { get; set; }
     
        public string Birimi { get; set; } 
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string PartiNo { get; set; }
        public string LotNo { get; set; }
        public Guid? UrId { get; set; }
        public Guid? SipId { get; set; }
        public Guid? FisId { get; set; }

        public MalKabulFisKullanilanStokModel Clone() {
            return (MalKabulFisKullanilanStokModel)MemberwiseClone();
        }
    }
}
