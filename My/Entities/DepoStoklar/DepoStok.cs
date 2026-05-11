using My.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.DepoStoklar {
    [Table("DepoStok")]
    public class DepoStok {
        public DepoStok Clone() { return (DepoStok)MemberwiseClone(); }
        [Key] public Guid? Id { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birimi { get; set; }
        public string Depo { get; set; }
        public string Bolge { get; set; }
        public string Raf { get; set; }
        public string Kat { get; set; }
        public string AnaGrup { get; set; }
        public string AltGrup { get; set; }
        public string StokCins { get; set; }
        public Guid? StGuid { get; set; }
        public string Barkodu { get; set; }
        [Ignore] public bool Sec { get; set; }
        public string KasaKoliTipi { get; set; }
        public double KasaAdedi { get; set; }


        public static string GetSelectSqlCode(string sor = "") {
            string sql = @"SELECT *  FROM DepoStok  " + sor;
            return sql;
        }
        public static string GetSelectSqlCodeById(Guid?  Id) {
            string sql = $@"SELECT *  FROM DepoStok  WITH (NOLOCK) where  Id='{Id}'";
            return sql;
        }
    }
}
