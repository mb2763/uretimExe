using System;
using System.Runtime.InteropServices;

namespace My.Entities.Mikro
{
    public class MikroStokMaliyet : IEntity
    {
        public Guid StGuid { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
        public string RenkKodu { get; set; }
        public string BedenKodu { get; set; }
        public string StokCinsi { get; set; }
        public string STANDARTMALIYET { get; set; }
        public string RECETEMALIYET { get; set; }

        [ComVisible(true)]
        public MikroStokMaliyet Clone()
        {
            return (MikroStokMaliyet)MemberwiseClone();
        }

        /*
            SELECT   TOP 100 PERCENT S.sto_Guid As StGuid,S.sto_kod AS StokKodu,S.sto_isim AS StokAdi,
            s.sto_birim1_ad as Birim,S.sto_renk_kodu As RenkKodu,S.sto_beden_kodu as BedenKodu,
            coalesce( S.sto_cins,0) as StokCinsi,
            coalesce( STU.STANDARTMALIYET,0) as STANDARTMALIYET,     coalesce( STU.RECETEMALIYET ,0) AS RECETEMALIYET
            FROM STOKLAR S
            LEFT JOIN STOKLAR_USER STU ON Record_uid=sto_Guid
         */
    }
}
