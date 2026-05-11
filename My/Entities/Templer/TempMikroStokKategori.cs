using My.Core.Data;
using System;

namespace My.Entities.Templer {

    [Table("TempMikroStokKategori")]
    public class TempMikroStokKategori {
        [Key] public Guid Id { get; set; }
       public string Turu { get; set; }
       public string KategoriKodu { get; set; }
        public string KategoriAdi { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EditDate { get; set; }

    }
}
