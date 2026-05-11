using My.Core.Data;
using System;

namespace My.Entities.ReceteIstasyonGruplar
{
    [Table("ReceteIstasyonGrupKod")]
    public class ReceteIstasyonGrupKod
    {
        [Key] public Guid? Id { get; set; }
        public string Kodu { get; set; }
        public string Adi { get; set; }
        public string Aciklama { get; set; }

        public ReceteIstasyonGrupKod Clone()
        {
                return (ReceteIstasyonGrupKod) MemberwiseClone();
        }
    }
}
