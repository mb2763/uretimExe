using My.Core.Data;
using System;

namespace My.Entities.SmsAyarlar {
    [Table("SmsAyar")]
    public class SmsAyar {
        [Key] public Guid? Id { get; set; }
        public string KullaniciKodu { get; set; }
        public string Sifre { get; set; }
        public string Baslik { get; set; }
        public string GunderimUrl { get; set; }
        public string RaporUrl { get; set; }

        public SmsAyar()
        {
            GunderimUrl = "https://api.netgsm.com.tr/sms/send/xml";
            RaporUrl = "https://api.netgsm.com.tr/sms/report";
        }
        public SmsAyar Clone() {

            return (SmsAyar)MemberwiseClone() ;
        }

    }
}
