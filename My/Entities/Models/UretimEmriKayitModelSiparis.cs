using My.Entities.UretimEmirler;
using My.Entities.UretimOperasyonlar;
using My.Entities.UretimStoklar;
using System.Collections.Generic;

namespace My.Entities.Models
{
    public class UretimEmriKayitModelSiparis
    {
        public UretimEmri UretimEmri { get; set; }


        public List<UretimStok> UretimStoklar { get; set; }
        public List<UretimOperasyon> UretimOperasyonlar { get; set; }
        public List<UretimOperasyonHareket> UretimOperasyonHareketler { get; set; }
        public List<ReceteKayitModel> ReceteModeller { get; set; }
        public List<ReceteOperasyonKayitModel> OperasyonModeller { get; set; }
        public SiparisKayitModel SiparisModel { get; set; }


    }
}