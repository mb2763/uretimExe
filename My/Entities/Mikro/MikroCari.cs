using System;
using System.Runtime.InteropServices;

namespace My.Entities.Mikro
{
    public class MikroCari
    {
        public string CariKodu { get; set; }
        public string CariUnvani1 { get; set; }
        public string CariUnvani2 { get; set; }
        public string SektorKodu { get; set; }
        public string AnaCariKodu { get; set; }
        public string GrupKodu { get; set; }
        public string Dvz { get; set; }
        public string Kargo { get; set; }
        public string Fason { get; set; }
        public string Email { get; set; }
        public Guid CrGuid { get; set; }

        [ComVisible(true)]
        public MikroCari Clone()
        {
            return (MikroCari) MemberwiseClone();
        }
    }
}