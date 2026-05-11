using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Raporlar {
    public class KaliteRaporModel {
           
        public string IsEmriNo { get; set; }
        public string ParcaNo { get; set; }
        public string ParcaAdi { get; set; }
        public string MalzemeKodu { get; set; }
        public string MalzemeAdi { get; set; }
        public string MalzemeYapisKodu { get; set; }
        public string MalzemeYapisAdi { get; set; }
        public string AmbalajSekli { get; set; }
        public DateTime TeslimTarihi { get; set; }
        public DateTime IsEmriTarihi { get; set; }
        public double TalepEdilenMiktar { get; set; }
        public double MalzemeMiktar { get; set; }
        public double MalzemeYapistiriciMiktar { get; set; }      
        public string LotNo { get; set; }
        public string MalzemeLotNo { get; set; }
        public string MalzemYapistiriciLotNo { get; set; }       

    }
}
