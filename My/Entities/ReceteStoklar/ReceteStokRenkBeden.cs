using My.Core;
using My.Core.Data;
using System;

namespace My.Entities.ReceteStoklar
{
    public class ReceteStokRenkBeden
    {
        [Ignore] public bool Sec { get; set; }
        [Key] public Guid? Id { get; set; }
        public string StokKodu { get; set; }
        public string Turu { get; set; }
        public string Kodu { get; set; }
        public double Miktar { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }
        public Guid? RcSTId { get; set; }


        public ReceteStokRenkBeden Clone()
        {
            return (ReceteStokRenkBeden)MemberwiseClone();
        }

        public static ReceteStokRenkBeden GetRenk(ReceteStok rst, string renk)
        {
            ReceteStokRenkBeden mdl = new ReceteStokRenkBeden();
            mdl.Id = MyGuid.NewGuid();
            mdl.StokKodu = rst.StokKodu;
            mdl.Turu = "Renk";
            mdl.Kodu = renk;
            mdl.RcAId = rst.RcAId;
            mdl.RcDId = rst.RcDId;
            mdl.RcSTId = rst.Id;
            return mdl;
        }
        public static ReceteStokRenkBeden GetBeden(ReceteStok rst, string beden)
        {
            ReceteStokRenkBeden mdl = new ReceteStokRenkBeden();
            mdl.Id = MyGuid.NewGuid();
            mdl.StokKodu = rst.StokKodu;
            mdl.Turu = "Beden";
            mdl.Kodu = beden;
            mdl.RcAId = rst.RcAId;
            mdl.RcDId = rst.RcDId;
            mdl.RcSTId = rst.Id;
            return mdl;
        }
    }
}
