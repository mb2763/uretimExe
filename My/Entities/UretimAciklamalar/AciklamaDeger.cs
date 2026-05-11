using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.UretimAciklamalar {
    [Table("AciklamaDeger")]
    public class AciklamaDeger {
        [Key] public Guid? Id { get; set; } 
        public string Modul { get; set; }
        public string Kodu { get; set; }
        public int Sira { get; set; }
        public string Deger1 { get; set; }
        public string Deger2 { get; set; }
        public string Deger3 { get; set; }
        public string EntKodu { get; set; }
        public Guid? EntId { get; set; }

        [ComVisible(true)]
        public AciklamaDeger Clone() {
            return (AciklamaDeger)MemberwiseClone();
        }
    }
     
}
