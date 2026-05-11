using System; 
using My.Core.Data;

namespace My.Entities.Models
{
    public class MailEklentiModel
    {
        [Key]
        public Guid? Id { get; set; }
        public byte[] Eklenti { get; set; }
        public short Sira { get; set; }
        public string DosyaAdi { get; set; } 
        public Guid? MailId { get; set; }
        public short Silindi { get; set; }

        public MailEklentiModel Clone()
        {
            return (MailEklentiModel)this.MemberwiseClone();
        }
    }
}
