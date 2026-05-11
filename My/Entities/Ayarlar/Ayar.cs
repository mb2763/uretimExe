using My.Core.Data;
using System;

namespace My.Entities.Ayarlar {

    [Table("Ayar")]
    public class Ayar {     
        [Key] public Guid? Id { get; set; }
        public string Modul { get; set; }
        public string Grup { get; set; }
        public string Kodu { get; set; }
        public string Aciklama { get; set; } 
        public string Deger { get; set; } 

        public Ayar Clone() {
            return(Ayar)MemberwiseClone();
        }


    }
}
