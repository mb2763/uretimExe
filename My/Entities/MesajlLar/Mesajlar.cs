using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.MesajLar {
    [Table("Mesajlar")]
    public class Mesajlar {
        [Key] public Guid? Id { get; set; }
        public string Modul { get; set; }
        public string Kodu { get; set; }
        public string Personel { get; set; }
        public string Mesaj { get; set; }
        public DateTime? Tarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        [ComVisible(true)]
        public Mesajlar Clone() {
            return (Mesajlar)MemberwiseClone();
        }
    }
}
