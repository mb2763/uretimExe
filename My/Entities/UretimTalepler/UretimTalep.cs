using My.Core;
using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.UretimTalepler {

    [Table("UretimTalep")]
    public class UretimTalep {
     
        [Key] public Guid UrtTlpId { get; set; } 
        public DateTime? Tarih { get; set; }  
        public string EvrakNo { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string Kullanici { get; set; }
        public string Aciklama { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public short Ent { get; set; }
        public Guid? EntId { get; set; }
        public string EntKodu { get; set; }
        public string EntKodu2 { get; set; }
        public DateTime? EntTarih { get; set; }
         
        [ComVisible(true)]
        public UretimTalep Clone() {
            return (UretimTalep)MemberwiseClone();
        }

    }
}
