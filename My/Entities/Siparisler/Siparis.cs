using System;
using My.Core;
using My.Core.Data;

namespace My.Entities.Siparisler
{
    [Table("Siparis")]
    public class Siparis
    {
        public Siparis()
        {
            Id = MyGuid.NewGuid();
            SiparisKodu = "";
            Tarih = DateTime.Now;
            TeslimTarihi = DateTime.Now; 
        } 
        [Key] public Guid? Id { get; set; } 
        public string Turu { get; set; }
        public string SiparisKodu { get; set; }
       
        public string CariKodu { get; set; }
        public string CariUnvani { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public double Miktar { get; set; }
        public string Aciklama { get; set; }
        public string Notu { get; set; }
        public bool Kapandi { get; set; }
        public string Durumu { get; set; }  
        public string Kargo { get; set; }
        public string Email { get; set; } 
        public bool Ent { get; set; } 
        public string EntSeri { get; set; }
        public string EntSira { get; set; }
        public bool EntIptal { get; set; } 
        public string EntIptalSeri { get; set; }
        public string EntIptalSira { get; set; }
        public string EntCode { get; set; } 
        public DateTime? EntDate { get; set; }
        public string EntKayitSeri { get; set; }
        public string EntKayitSira { get; set; }
        public Guid? EntKayitGuid { get; set; } 
        public bool EtiketBasildi { get; set; }
        [Ignore]
        public string IsEmriNo { get; set; }

    }
}