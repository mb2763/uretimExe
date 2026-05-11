using My.Core.Data;
using System;

namespace My.Entities.Mikro
{
    public class StokHareketleriModel
    {
        public Guid sth_Guid { get; set; }
        public string sth_evrakno_seri { get; set; }
        public int sth_evrakno_sira { get; set; }
        public DateTime? sth_tarih { get; set; }
        public string sth_belge_no { get; set; }
        public DateTime? sth_belge_tarih { get; set; }
        public string sth_stok_kod { get; set; } 
        public double sth_miktar { get; set; }
        public double sth_miktar2 { get; set; }
        public int sth_satirno { get; set; }
        public int sth_giris_depo_no { get; set; }
        public int sth_cikis_depo_no { get; set; }
        public DateTime? sth_malkbl_sevk_tarihi { get; set; }
        public short sth_birim_pntr { get; set; }
        public string sth_aciklama { get; set; } 
        public double Fiyat { get; set; }  
        public string sth_parti_kodu { get; set; }
        public int sth_lot_no { get; set; } 
        public string sth_isemri_gider_kodu { get; set; }
        public string sth_proje_kodu { get; set; }
        public string sth_stok_srm_merkezi { get; set; }

        /// <summary>
        /// StokVirman fişinde kullanılıyor
        /// </summary>
        public short sth_tip { get; set; } 
        public string Renk { get; set; }
        public string Beden { get; set; }
        public string IsEmriNo { get; set; }
        public string IsEmriKodu { get; set; }
        public string StokAdi { get; set; }
        public DateTime? UretimTarihi { get; set; }
        public DateTime? SonKullanmaTarihi { get; set; } 

        public string sth_special1 { get; set; }
        public string sth_special2 { get; set; }
        public string sth_special3 { get; set; }


        public StokHareketleriModel Clone() {
            return (StokHareketleriModel) MemberwiseClone();
        }
         

    }
}