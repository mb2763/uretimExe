using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.ReceteIstasyonlar
{
    [Table("ReceteIstasyon")]
    public class ReceteIstasyon
    {
        public ReceteIstasyon()
        {
            Id = MyGuid.NewGuid() ;
            OperasyonKodu = "";
            OperasyonAdi = "";
            IstasyonKodu = "";
            IstasyonAdi = "";
            Fason = false;
            RcAId = Guid.Empty;
            RcOId = Guid.Empty;
            
        }

        [Key] public Guid? Id { get; set; }

        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public double MaliyetFiyat { get; set; }
        public bool Fason { get; set; }

        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }

        [ComVisible(true)]
        public ReceteIstasyon Clone()
        {
            return (ReceteIstasyon) MemberwiseClone();
        }

        public void SetDetay(ReceteIstasyon dty)
        {
            Id = dty.Id;
            OperasyonKodu = dty.OperasyonKodu;
            OperasyonAdi = dty.OperasyonAdi;
            IstasyonKodu = dty.IstasyonKodu;
            IstasyonAdi = dty.IstasyonAdi;
            Fason = dty.Fason;
            RcAId = dty.RcAId;
            RcOId = dty.RcOId;
           
        }
    }
}