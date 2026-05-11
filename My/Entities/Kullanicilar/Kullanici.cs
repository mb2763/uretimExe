using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Kullanicilar
{
    [Table("Kullanici")]
    public class Kullanici
    {
        public Kullanici()
        {
            Id = MyGuid.NewGuid();
            Adi = "";
            Soyadi = "";
            KullaniciAdi = "";
            Sifre = "";
            Admin = false;
        }

        [Key] public Guid Id { get; set; }

        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public bool Admin { get; set; }

        [ComVisible(true)]
        public Kullanici Clone()
        {
            return (Kullanici) MemberwiseClone();
        }
    }
}