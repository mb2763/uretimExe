using My.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.IstasyonTakipler {



    [Table("IstasyonTakipHareketDetay")]
    public class IstasyonTakipHareketDetay {

        [Ignore] public bool Sec { get; set; }

        [Key]
        public Guid? Id { get; set; }
        public Guid? IstHrId { get; set; }
        public Guid? UrId { get; set; }
        public Guid? UrIId { get; set; } 
        [Ignore] public string SiparisKodu { get; set; }
        [Ignore] public string Durumu { get; set; }
        [Ignore] public string IstasyonKodu { get; set; }
        [Ignore] public string IstasyonAdi { get; set; }
        [Ignore] public string OperasyonKodu { get; set; }
        [Ignore] public string OperasyonAdi { get; set; }
        [Ignore] public string ReceteKodu { get; set; }
        [Ignore] public string ReceteAdi { get; set; } 
        public DateTime? Tarih { get; set; }
        public string Turu { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public double Miktar { get; set; }
        public double FireMiktar { get; set; }
        public double IptalMiktar { get; set; }
        public string Kodu { get; set; }
        public string Aciklama { get; set; }
        public string Personel { get; set; }
        public string AdiSoyadi { get; set; }
        public string Birim { get; set; }
        public string Birim2 { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string Parti { get; set; }
        public string Lot { get; set; }
        public string KayitEden { get; set; }
        public DateTime? KayitTarihi { get; set; }
        public bool Ent { get; set; }
        public string EntCode { get; set; }
        public DateTime? EntDate { get; set; }
        public string EntSeri { get; set; }
        public string EntSira { get; set; }
        public double OlcumDegeri { get; set; }
         
        public IstasyonTakipHareketDetay Clone() { 
            return (IstasyonTakipHareketDetay)MemberwiseClone();
        } 
        public static string GetUpdateSqlCode() { 
            string sql = $@"  
     UPDATE IstasyonTakipHareketDetay SET 
        Miktar      =  @Miktar
       ,FireMiktar =  @FireMiktar
       ,IptalMiktar =  @IptalMiktar 
     WHERE Id=@Id;  ";
            return sql;
        }

    }

}
