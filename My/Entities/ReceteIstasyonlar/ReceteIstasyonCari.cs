using System;
using System.Runtime.InteropServices;
using My.Core.Data;

namespace My.Entities.ReceteIstasyonlar
{
    [Table("ReceteIstasyonCari")]
    public class ReceteIstasyonCari
    {
        [Key] public Guid? Id { get; set; }

        public string CariKodu { get; set; }
        public string CariUnvani { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcOId { get; set; }
        public Guid? RcIstId { get; set; } 

        [ComVisible(true)]
        public ReceteIstasyonCari Clone()
        {
            return (ReceteIstasyonCari) MemberwiseClone();
        }
    }
}