using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Raporlar {
    public class ReceteRaporuModel {

        public int FisSira { get; set; }
        public int ReceteSira { get; set; }

        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }

        public string Cinsi { get; set; }
        public string CinsAdi { get; set; }


        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
            public double Miktar { get; set; }
      
        public double FireYuzde { get; set; }
        public double FireliMiktar { get; set; }
     
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }
        

    }
}
