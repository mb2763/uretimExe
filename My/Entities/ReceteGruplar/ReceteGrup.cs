using System;
using System.Runtime.InteropServices;
using My.Core;
using My.Core.Data;

namespace My.Entities.ReceteGruplar
{
    [Table("ReceteGrup")]
    public class ReceteGrup
    {
        public ReceteGrup()
        {
            Id = MyGuid.NewGuid();
            Grubu = "";
            ReceteGrupKodu = "";
            Aciklama = "";
        }

        [Key] public Guid? Id { get; set; } 
        public string Grubu { get; set; } 
        public string ReceteGrupKodu { get; set; } 
        public string Aciklama { get; set; }

        [ComVisible(true)]
        public ReceteGrup Clone()
        {
            return (ReceteGrup) MemberwiseClone();
        }
    }
}