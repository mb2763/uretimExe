using System;
using System.Runtime.InteropServices;
using My.Core.Data;

namespace My.Entities.OperasyonKartlar
{
    [Table("OperasyonKarti")]
    public class OperasyonKarti
    {
        [Key] public Guid? Id { get; set; }

        public string OperasyonKodu { get; set; }
        public string OperasyonAdi { get; set; }
        public string VarsayilanIstasyonKodu { get; set; }
        public string VarsayilanIstasyonAdi { get; set; }

        [ComVisible(true)]
        public OperasyonKarti Clone()
        {
            return (OperasyonKarti) MemberwiseClone();
        }
    }
}