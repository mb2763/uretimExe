namespace My.Entities.Models
{
    public  class ReceteMaliyetGenelDetayModel
    { 
        public string Cinsi { get; set; } 
        public string VarsayilanStokKodu { get; set; }
        public string VarsayilanStokAdi { get; set; }
        public string Birim { get; set; } 
        public double Miktar { get; set; }
        public double Fiyat { get; set; }
        public double Tutar { get; set; }  
        public string Aciklama { get; set; }
        public bool StokKullan { get; set; }
        public int ReceteSira { get; set; } 
        public string StokTuru { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }

    }
}
