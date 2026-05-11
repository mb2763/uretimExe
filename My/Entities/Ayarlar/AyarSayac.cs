using System;
using My.Core.Data;

namespace My.Entities.Ayarlar
{
    [Table("AyarSayac")]
    public class AyarSayac
    {
        [Key] public Guid Id { get; set; } 
        public string Kodu { get; set; }
        public string Aciklama { get; set; }
        public int BasamakSayisi { get; set; }
        public string BasinaEkle { get; set; }
        public int Verilecek { get; set; }
    }
}