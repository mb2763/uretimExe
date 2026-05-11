using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.IstasyonAciklamalar {
    [Table("IstasyonAciklama")]
    public class IstasyonAciklama {
        [Key] public Guid? Id { get; set; } 
        public string Modul { get; set; }
        public string Kodu { get; set; }
        public string Deger { get; set; }
        public bool SmsGonder { get; set; } 
        public string SmsKodu { get; set; }
        public string Gorevi { get; set; }

        [ComVisible(true)]
        public IstasyonAciklama Clone() {
            return (IstasyonAciklama)MemberwiseClone();
        }
    }

    public  enum IstasyonAciklamaModulTuru {
        IstasyonBaslatmaHata,
        IstasyonDurdurmaKodu,
        IstasyonFireSebep 
    }
}
