using System;
using System.Runtime.InteropServices;
using My.Core.Data;

namespace My.Entities.IstasyonKartlar
{
    [Table("IstasyonKarti")]
    public class IstasyonKarti
    {
        [Key] public Guid? Id { get; set; } 
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public string Operasyon { get; set; }
        public string OperasyonAdi { get; set; }
        public bool KaliteKontrol { get; set; }
        public bool Fason { get; set; }
        public bool Yazdirilmali { get; set; }
        public string FasonCariKodu { get; set; }
        public string FasonCariAdi { get; set; }

        [ComVisible(true)]
        public IstasyonKarti Clone()
        {
            return (IstasyonKarti) MemberwiseClone();
        }
    }
}