using My.Core.Data;
using System;
using System.Runtime.InteropServices;

namespace My.Entities.IstasyonBakimlar
{
    [Table("IstasyonBakimParca")]
    public class IstasyonBakimParca
    {
        [Key] public Guid? Id { get; set; }
        public Guid? IstBakId { get; set; }
        public string Parca { get; set; }
        public string ParcaNo { get; set; }
        public string EvrakNo { get; set; }
        public string Aciklama { get; set; }
        public bool Garanti { get; set; }


        [ComVisible(true)]
        public IstasyonBakimParca Clone()
        {
            return (IstasyonBakimParca)MemberwiseClone();
        }


    }
}
