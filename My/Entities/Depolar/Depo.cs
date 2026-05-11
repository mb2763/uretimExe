using My.Core.Data;
using My.Entities.Ayarlar;
using System; 

namespace My.Entities.Depolar {
    [Table("Depo")]
    public class Depo {
        [Key] public Guid? Id { get; set; }
        public string DepoKodu { get; set; }
        public string DepoAdi { get; set; } 
        public int MikroDepoNo { get; set; } 
        public string Kullanicilar { get; set; }
        public Depo Clone() {
            return (Depo)MemberwiseClone();
        } 
    }
}
