using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.DepoStoklar {
    public class DepoStokCikisSiraKontrolModel {
        public Guid? StGuid { get; set; }
        public int Sira { get; set; }
        public double Kalan { get; set; }
        public double Cikan { get; set; }
        public string BarGui { get; set; }
        public int SiraEski { get; set; }
    }
}
