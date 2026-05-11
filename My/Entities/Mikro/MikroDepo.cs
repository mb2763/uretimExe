using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Mikro {
    public class MikroDepo : IEntity {
        public int DepoNo { get; set; }
        public string DepoAdi { get; set; }
     

        [ComVisible(true)]
        public MikroDepo Clone() {
            return (MikroDepo)MemberwiseClone();
        }
    }
}
