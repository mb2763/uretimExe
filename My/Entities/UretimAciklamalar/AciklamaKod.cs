using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.UretimAciklamalar {
    [Table("AciklamaKod")]
    public class AciklamaKod {
        [Key] public Guid? Id { get; set; } 
        public string Modul { get; set; }
        public string Kodu { get; set; } 
        public string Deger1 { get; set; }
        public string Deger2 { get; set; }
        public string Deger3 { get; set; }
        public int Sira { get; set; }

        [ComVisible(true)]
        public AciklamaKod Clone() {
            return (AciklamaKod)MemberwiseClone();
        }
    }

    public  enum AciklamaModulTuru {
         ReceteAciklama,
         OperasyonAciklama,

    }
}
