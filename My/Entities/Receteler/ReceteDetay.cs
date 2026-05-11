using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.Receteler
{
    [Table("ReceteDetay")]
    public class ReceteDetay
    { 
        [Key] public Guid? Id { get; set; }
        public string Cinsi { get; set; }
        public int ReceteSira { get; set; }
        public bool StokKullan { get; set; }
        public string StokTuru { get; set; }
     
        public string VarsayilanStokKodu { get; set; }
        public string VarsayilanStokAdi { get; set; }
        public string Birim { get; set; }
        public double FireYuzde { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Ebat { get; set; }
        public string Gram { get; set; }
        public string Olcu { get; set; }
        public double Miktar { get; set; }      
        public string Aciklama { get; set; }
        public string StokAnaGrup { get; set; }
        public string StokAltGrup { get; set; }
        public bool SiparisdeGosterme { get; set; } 
        public Guid? RcAId { get; set; }
        public double OperasyonMaliyet { get; set; }
    

        public ReceteDetay()
        {
            Id = MyGuid.NewGuid();
            Cinsi = "";
            ReceteSira = 0;
            StokKullan = true;
            StokTuru = "Sabit";
            StokAnaGrup = "";
            StokAltGrup = "";
            VarsayilanStokKodu = "";
            VarsayilanStokAdi = "";
            Birim = "";
            Renk = "";
            Beden = "";
            Miktar = 0;
            FireYuzde = 0;
            Aciklama = "";
            SiparisdeGosterme = true;
            RcAId = Guid.Empty;

        }
        [ComVisible(true)]
        public ReceteDetay Clone()
        {
            return (ReceteDetay) MemberwiseClone();
        }

        public void SetDetay(ReceteDetay dty)
        {
            Id = dty.Id;
            Cinsi = dty.Cinsi;
            ReceteSira = dty.ReceteSira;
            StokKullan = dty.StokKullan;
            StokTuru = dty.StokTuru;
            StokAnaGrup = dty.StokAnaGrup;
            StokAltGrup = dty.StokAltGrup;
            VarsayilanStokKodu = dty.VarsayilanStokKodu;
            VarsayilanStokAdi = dty.VarsayilanStokAdi;
            Birim = dty.Birim;
            Renk = dty.Renk;
            Beden = dty.Beden;
            Miktar = dty.Miktar;
            Aciklama = dty.Aciklama;
            SiparisdeGosterme = dty.SiparisdeGosterme;
            RcAId = dty.RcAId;
            FireYuzde=dty.FireYuzde;
        }
    }
}