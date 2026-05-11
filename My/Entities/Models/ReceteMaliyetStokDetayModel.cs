namespace My.Entities.Models {
    public class ReceteMaliyetStokDetayModel {
        public string Cinsi { get; set; }
        public string VarsayilanStokKodu { get; set; }
        public string VarsayilanStokAdi { get; set; }
        public string Birim { get; set; }
        public double Miktar { get; set; }
      
        public double SonAlis { get; set; }
        public double Son5AlisOrtalama { get; set; }
        public double StandartMaliyet { get; set; }
        public double SonSayimGiris { get; set; }
        public double DevirGiris { get; set; } 
        public double SonAlisTutar { get; set; }
        public double Son5AlisTutar { get; set; }
        public double StandartMaliyetTutar { get; set; }
        public double SonSayimGirisTutar { get; set; }
        public double DevirGirisTutar { get; set; }
        public string Aciklama { get; set; }
        public bool StokKullan { get; set; }
        public int ReceteSira { get; set; }
        public string StokTuru { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
    }
}
