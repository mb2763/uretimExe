using System;

namespace My.Entities.Mikro
{
    public class MikroSiparisHareket
    {
        public string EvrakSeri { get; set; }
        public string EvrakSira { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public string Birim { get; set; }
        public string TalepTemin { get; set; }
        public string HareketTipi { get; set; }
        public string SiparisCins { get; set; }
        public DateTime? Tarih { get; set; }
        public DateTime? TeslimTarihi { get; set; }
        public double KalanMiktar { get; set; }
        public string CariKodu { get; set; }
        public string Aciklama { get; set; }
        public string DepoNo { get; set; }
        public string DepoIsmi { get; set; }
        public string SipBirimPntr { get; set; }
        public Guid SipHGuid { get; set; }
    }
}