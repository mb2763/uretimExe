namespace My.Entities.Models
{
    public class MikroReceteModel
    {
        //   ReceteKodu,   AnaBirimi,    AnaMiktar
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; }
        public string AnaBirimi { get; set; }
        public double AnaMiktar { get; set; } 
        public bool Aktarildi { get; set; }
        public bool StokAktif { get; set; } 
        //public double FireYuzde { get; set; }

    }
}