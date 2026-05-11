using My.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Receteler {
   
    public class ReceteKullanilanStokModel {
        //[Key] public Guid? Id { get; set; }
     
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; } 
        public string VarsayilanStokKodu { get; set; }
        public string VarsayilanStokAdi { get; set; }
        public string Birim { get; set; }
        public string Cinsi { get; set; }
        public string StokTuru { get; set; }
        public int ReceteSira { get; set; }
        public bool StokKullan { get; set; }
        public double FireYuzde { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Ebat { get; set; }
        public string Gram { get; set; }
        public string Olcu { get; set; }
        public double Miktar { get; set; }
        public string Aciklama { get; set; }
        public string StokAnaGrup { get; set; }
        public string StokAltGrup { get; set; }
        public bool SiparisdeGosterme { get; set; }
         public Guid? RcAId { get; set; }
        //public double OperasyonMaliyet { get; set; }

    }
    }
