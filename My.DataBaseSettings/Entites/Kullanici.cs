using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using My.Core.Data;

namespace My.DatabaseSettings.Entites {
    [Table("Kullanici")]
    public class Kullanici {
        public Kullanici() {
            Id = Guid.Empty;
            Adi = "";
            Soyadi = "";
            KullaniciAdi = "";
            Sifre = "";
            Admin = false;
        }

        [Key] public Guid? Id { get; set; }

        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public bool Admin { get; set; }

        [ComVisible(true)]
        public Kullanici Clone() {
            return (Kullanici)MemberwiseClone();
        }
    }
}
