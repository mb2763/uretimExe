using My.Core.Data;
using System;

namespace My.Entities.Templer {

    [Table("TempMikroStok")]
    public class TempMikroStok {
        [Key] public Guid Id { get; set; }
       public string StokKodu { get; set; }
        public string StokAdi { get; set; }  
        public short CinsKodu { get; set; } 
        public string Cinsi { get; set; } 
        public string KategoriKodu { get; set; }
        public string KategoriAdi { get; set; } 
        public string KaliteKontrolKodu { get; set; }
        public string KaliteKontrolAdi { get; set; }
        public string ReyonKodu { get; set; }
        public string ReyonAdi { get; set; } 
        public string Birim1 { get; set; }
        public string Birim2 { get; set; }
        public string Birim3 { get; set; }
        public string Birim4 { get; set; }
        public double Katsayi1 { get; set; }
        public double Katsayi2 { get; set; }
        public double Katsayi3 { get; set; }
        public double Katsayi4 { get; set; } 
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; } 
        public int TakipTip { get; set; }
        public int RbTakipTip { get; set; }

    }
}
