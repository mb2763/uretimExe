using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Entities.Mikro {
    public class MikroStokHareketAktarilanFisler {


        public bool Sec { get; set; }

        public string BelgeNo { get; set; }
        public string EvrakSeri { get; set; }
        public string EvrakSira { get; set; }
        public string EvrakIsim { get; set; }
        public string CinsIsim { get; set; }
        public string TipIsim { get; set; }
        public string NormalIadeIsim { get; set; }

        public DateTime? Tarih { get; set; }
        public DateTime? BelgeTarih { get; set; }


        public static string GetSelectSqlCodeByBelgeNo(string belgeno,string deposevkHaricSql) {
            string sql = $@" SELECT 0 as Sec,   SH.sth_belge_no AS BelgeNo,
 SH.sth_evrakno_seri AS EvrakSeri,SH.sth_evrakno_sira AS EvrakSira,
TMPEVR.SHEvrIsim AS EvrakIsim,
TMPCNS.SHCinsIsim AS CinsIsim,
TMPTIP.SHTipIsim AS TipIsim,
TMPNIA.NIIsim AS NormalIadeIsim ,
SH.sth_tarih as Tarih,SH.sth_belge_tarih as BelgeTarih

FROM dbo.STOK_HAREKETLERI SH WITH (NOLOCK) 
LEFT OUTER JOIN dbo.vw_Stok_Hareket_Evrak_Isimleri TMPEVR  ON TMPEVR.SHEvrNo=sth_evraktip
LEFT OUTER JOIN dbo.vw_Stok_Hareket_Cins_Isimleri  TMPCNS  ON TMPCNS.SHCinsNo=sth_cins
LEFT OUTER JOIN dbo.vw_Stok_Hareket_Tip_Isimleri   TMPTIP  ON TMPTIP.SHTipNo=sth_tip
LEFT OUTER JOIN dbo.vw_Normal_Iade_Isimleri        TMPNIA  ON TMPNIA.NINo=sth_normal_iade

WHERE   sth_belge_no='{belgeno}'  {deposevkHaricSql}
GROUP BY SH.sth_belge_no,SH.sth_evrakno_seri,SH.sth_evrakno_sira, 
 TMPEVR.SHEvrIsim ,  TMPCNS.SHCinsIsim  , TMPTIP.SHTipIsim  , TMPNIA.NIIsim,SH.sth_tarih,SH.sth_belge_tarih    ";
            return sql;
        }
        //short sth_tip = 2; // Depo Sevk Giris
        //short sth_cins = 6;
        //short sth_evraktip = 2;
        public static string GetDeleteSqlCodeBySeriSira(string seri,string sira) {
            string sql = $@"  Delete  FROM dbo.STOK_HAREKETLERI   WHERE   sth_evrakno_seri='{seri}' and sth_evrakno_sira='{sira}'  ";
            return sql;
        }
      public static string GetDeleteSqlCodeByBelgeNo(string belgeno ) {
            string sql = $@"  Delete  FROM dbo.STOK_HAREKETLERI   WHERE   sth_belge_no='{belgeno}'    ";
            return sql;
        }
    }
}
