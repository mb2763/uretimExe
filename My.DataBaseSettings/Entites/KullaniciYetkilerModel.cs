
using System;

namespace My.DatabaseSettings.Entites {
    public class KullaniciYetkilerModel {
        public bool Durum { get; set; }
        public string Modul { get; set; }
        public string Yetki { get; set; }
        public string Aciklama { get; set; }
        public Guid? YetId { get; set; }
        public Guid? KulId { get; set; } 
    }
}
