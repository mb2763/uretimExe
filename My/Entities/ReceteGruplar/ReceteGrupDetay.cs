using System;
using My.Core;
using My.Core.Data;

namespace My.Entities.ReceteGruplar
{
    [Table("ReceteGrupDetay")]
    public class ReceteGrupDetay
    {
        public ReceteGrupDetay()
        {
            Id = MyGuid.NewGuid();
            RcAId = Guid.Empty;
            RcGId = Guid.Empty;
            ReceteKodu = "";
            ReceteAdi = "";
            Miktar = 0;
            Aciklama = "";
        }

        [Key] public Guid? Id { get; set; } 
        public string ReceteKodu { get; set; }
        public string ReceteAdi { get; set; } 
        public double Miktar { get; set; } 
        public string Aciklama { get; set; }
        public Guid? RcGId { get; set; } 
        public Guid? RcAId { get; set; }
       
    }
}