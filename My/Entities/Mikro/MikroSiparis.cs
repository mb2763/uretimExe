using System;

namespace My.Entities.Mikro
{
    public class MikroSiparis
    {
        public Guid SipGuid { get; set; }
        public string EvrakSeri { get; set; }
        public string EvrakSira { get; set; }
        public string CariKodu { get; set; }
        public string CariUnvani { get; set; }
        public string FirmaUnvan { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public double Miktar { get; set; }
        public string SiparisTip { get; set; }
        public string SiparisCins { get; set; }
        public string SiparisAcikKapali { get; set; }
        public bool Aktarildi { get; set; }
    }
}