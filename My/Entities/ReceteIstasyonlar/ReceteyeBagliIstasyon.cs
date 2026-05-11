using My.Core;
using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.ReceteIstasyonlar
{
    [Table("ReceteyeBagliIstasyon")]
    public class ReceteyeBagliIstasyon {
        public ReceteyeBagliIstasyon() {
            Id = MyGuid.NewGuid();
            IstasyonKodu = "";
            IstasyonAdi = "";
            RcAId = Guid.Empty;
            RcIId = Guid.Empty;
        }
        [Key] public Guid? Id { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcIId { get; set; }
        [ComVisible(true)]
        public ReceteyeBagliIstasyon Clone() {
            return (ReceteyeBagliIstasyon)MemberwiseClone();
        }
        public void SetDetay(ReceteyeBagliIstasyon dty) {
            Id = dty.Id;
            IstasyonKodu = dty.IstasyonKodu;
            IstasyonAdi = dty.IstasyonAdi;
            RcAId = dty.RcAId;
            RcIId = dty.RcIId;
        }
    }
}
