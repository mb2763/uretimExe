using My.Core.Data;
using System;

namespace My.Entities.ReceteIstasyonGruplar
{ 
    [Table("ReceteIstasyonGrupIstasyon")]
    public class ReceteIstasyonGrupIstasyon
    { 
        [Key] public Guid? Id { get; set; }
        public Guid RcAId { get; set; }
        public string GrupKodu { get; set; } 
        public ReceteIstasyonGrupIstasyon Clone()
        {
            return (ReceteIstasyonGrupIstasyon)MemberwiseClone();
        }
    }
}
