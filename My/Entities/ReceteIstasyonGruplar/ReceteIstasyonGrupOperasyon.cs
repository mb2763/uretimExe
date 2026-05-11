using My.Core.Data;
using System;

namespace My.Entities.ReceteIstasyonGruplar
{
    [Table("ReceteIstasyonGrupOperasyon")]
    public class ReceteIstasyonGrupOperasyon
    {

        [Key] public Guid? Id { get; set; } 
        public string GrupKodu { get; set; }
        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string IstasyonKodu { get; set; }
        public string IstasyonAdi { get; set; }

        public ReceteIstasyonGrupOperasyon Clone()
        {
            return (ReceteIstasyonGrupOperasyon)MemberwiseClone();
        }


    }
}
