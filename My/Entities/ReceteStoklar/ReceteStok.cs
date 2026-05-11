using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.ReceteStoklar
{
    [Table("ReceteStok")]
    public class ReceteStok {
        [Key] public Guid? Id { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Ebat { get; set; }
        public string Gram { get; set; }
        public string Olcu { get; set; }

        public double Miktar { get; set; }

        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }

        [ComVisible(true)]
        public ReceteStok Clone() {
            return (ReceteStok)MemberwiseClone();
        }
    }
}