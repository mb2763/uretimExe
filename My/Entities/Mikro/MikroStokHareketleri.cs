using System;
using My.Core.Data;

namespace My.Entities.Mikro
{
    [Table("STOK_HAREKETLERI")]
    public class MikroStokHareketleri
    {
        public MikroStokHareketleri(Guid _sth_Guid, string _sth_evrakno_seri, int _sth_evrakno_sira, string _sth_belge_no,
            string _sth_stok_kod, string _sth_aciklama, double? _sth_miktar, double? _sth_miktar2,
            int? _sth_giris_depo_no,
            int? _sth_cikis_depo_no, short? _sth_birim_pntr, int _sth_satirno,
            DateTime? _sth_tarih = null,
            DateTime? _sth_belge_tarih = null,
            DateTime? _sth_malkbl_sevk_tarihi = null)
        {
            sth_Guid = _sth_Guid;
            sth_evrakno_seri = _sth_evrakno_seri;
            sth_evrakno_sira = _sth_evrakno_sira;
            sth_belge_no = _sth_belge_no;
            sth_stok_kod = _sth_stok_kod;
            sth_aciklama = _sth_aciklama;
            sth_miktar = _sth_miktar;
            sth_miktar2 = _sth_miktar2;
            sth_giris_depo_no = _sth_giris_depo_no;
            sth_cikis_depo_no = _sth_cikis_depo_no;
            sth_birim_pntr = _sth_birim_pntr;
            sth_satirno = _sth_satirno;
            if (_sth_tarih == null) _sth_tarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            if (_sth_belge_tarih == null) _sth_belge_tarih = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            if (_sth_malkbl_sevk_tarihi == null) _sth_malkbl_sevk_tarihi = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            sth_tarih = _sth_tarih;
            sth_belge_tarih = _sth_belge_tarih;
            sth_malkbl_sevk_tarihi = _sth_malkbl_sevk_tarihi;
            sth_create_date = DateTime.Now;
            sth_lastup_date = DateTime.Now;
            Temizle();
        }

        [Key] public Guid sth_Guid { get; set; }

        public short sth_DBCno { get; set; }
        public int? sth_SpecRECno { get; set; }
        public bool? sth_iptal { get; set; }
        public short? sth_fileid { get; set; }
        public bool? sth_hidden { get; set; }
        public bool? sth_kilitli { get; set; }
        public bool? sth_degisti { get; set; }
        public int? sth_checksum { get; set; }
        public short? sth_create_user { get; set; }
        public DateTime sth_create_date { get; set; }
        public short? sth_lastup_user { get; set; }
        public DateTime? sth_lastup_date { get; set; }
        public string sth_special1 { get; set; }
        public string sth_special2 { get; set; }
        public string sth_special3 { get; set; }
        public int? sth_firmano { get; set; }
        public int? sth_subeno { get; set; }
        public DateTime? sth_tarih { get; set; }
        public short? sth_tip { get; set; }
        public short? sth_cins { get; set; }
        public short? sth_normal_iade { get; set; }
        public short? sth_evraktip { get; set; }
        public string sth_evrakno_seri { get; set; }
        public int? sth_evrakno_sira { get; set; }
        public int? sth_satirno { get; set; }
        public string sth_belge_no { get; set; }
        public DateTime? sth_belge_tarih { get; set; }
        public string sth_stok_kod { get; set; }
        public short? sth_isk_mas1 { get; set; }
        public short? sth_isk_mas2 { get; set; }
        public short? sth_isk_mas3 { get; set; }
        public short? sth_isk_mas4 { get; set; }
        public short? sth_isk_mas5 { get; set; }
        public short? sth_isk_mas6 { get; set; }
        public short? sth_isk_mas7 { get; set; }
        public short? sth_isk_mas8 { get; set; }
        public short? sth_isk_mas9 { get; set; }
        public short? sth_isk_mas10 { get; set; }
        public bool? sth_sat_iskmas1 { get; set; }
        public bool? sth_sat_iskmas2 { get; set; }
        public bool? sth_sat_iskmas3 { get; set; }
        public bool? sth_sat_iskmas4 { get; set; }
        public bool? sth_sat_iskmas5 { get; set; }
        public bool? sth_sat_iskmas6 { get; set; }
        public bool? sth_sat_iskmas7 { get; set; }
        public bool? sth_sat_iskmas8 { get; set; }
        public bool? sth_sat_iskmas9 { get; set; }
        public bool? sth_sat_iskmas10 { get; set; }
        public short? sth_pos_satis { get; set; }
        public bool? sth_promosyon_fl { get; set; }
        public short? sth_cari_cinsi { get; set; }
        public string sth_cari_kodu { get; set; }
        public short? sth_cari_grup_no { get; set; }
        public string sth_isemri_gider_kodu { get; set; }
        public string sth_plasiyer_kodu { get; set; }
        public short? sth_har_doviz_cinsi { get; set; }
        public double? sth_har_doviz_kuru { get; set; }
        public double? sth_alt_doviz_kuru { get; set; }
        public short? sth_stok_doviz_cinsi { get; set; }
        public double? sth_stok_doviz_kuru { get; set; }
        public double? sth_miktar { get; set; }
        public double? sth_miktar2 { get; set; }
        public short? sth_birim_pntr { get; set; }
        public double? sth_tutar { get; set; }
        public double? sth_iskonto1 { get; set; }
        public double? sth_iskonto2 { get; set; }
        public double? sth_iskonto3 { get; set; }
        public double? sth_iskonto4 { get; set; }
        public double? sth_iskonto5 { get; set; }
        public double? sth_iskonto6 { get; set; }
        public double? sth_masraf1 { get; set; }
        public double? sth_masraf2 { get; set; }
        public double? sth_masraf3 { get; set; }
        public double? sth_masraf4 { get; set; }
        public short? sth_vergi_pntr { get; set; }
        public double? sth_vergi { get; set; }
        public short? sth_masraf_vergi_pntr { get; set; }
        public double? sth_masraf_vergi { get; set; }
        public double? sth_netagirlik { get; set; }
        public int? sth_odeme_op { get; set; }
        public string sth_aciklama { get; set; }
        public Guid? sth_sip_uid { get; set; }
        public Guid? sth_fat_uid { get; set; }
        public int? sth_giris_depo_no { get; set; }
        public int? sth_cikis_depo_no { get; set; }
        public DateTime? sth_malkbl_sevk_tarihi { get; set; }
        public string sth_cari_srm_merkezi { get; set; }
        public string sth_stok_srm_merkezi { get; set; }
        public DateTime? sth_fis_tarihi { get; set; }
        public int? sth_fis_sirano { get; set; }
        public bool? sth_vergisiz_fl { get; set; }
        public double? sth_maliyet_ana { get; set; }
        public double? sth_maliyet_alternatif { get; set; }
        public double? sth_maliyet_orjinal { get; set; }
        public int? sth_adres_no { get; set; }
        public string sth_parti_kodu { get; set; }
        public int? sth_lot_no { get; set; }
        public Guid? sth_kons_uid { get; set; }
        public string sth_proje_kodu { get; set; }
        public string sth_exim_kodu { get; set; }
        public short? sth_otv_pntr { get; set; }
        public double? sth_otv_vergi { get; set; }
        public double? sth_brutagirlik { get; set; }
        public short? sth_disticaret_turu { get; set; }
        public double? sth_otvtutari { get; set; }
        public bool? sth_otvvergisiz_fl { get; set; }
        public short? sth_oiv_pntr { get; set; }
        public double? sth_oiv_vergi { get; set; }
        public bool? sth_oivvergisiz_fl { get; set; }
        public int? sth_fiyat_liste_no { get; set; }
        public double? sth_oivtutari { get; set; }
        public short? sth_Tevkifat_turu { get; set; }
        public int? sth_nakliyedeposu { get; set; }
        public short? sth_nakliyedurumu { get; set; }
        public Guid? sth_yetkili_uid { get; set; }
        public bool? sth_taxfree_fl { get; set; }
        public double? sth_ilave_edilecek_kdv { get; set; }
        public string sth_ismerkezi_kodu { get; set; }
        public string sth_HareketGrupKodu1 { get; set; }
        public string sth_HareketGrupKodu2 { get; set; }
        public string sth_HareketGrupKodu3 { get; set; }
        public double? sth_Olcu1 { get; set; }
        public double? sth_Olcu2 { get; set; }
        public double? sth_Olcu3 { get; set; }
        public double? sth_Olcu4 { get; set; }
        public double? sth_Olcu5 { get; set; }
        public short? sth_FormulMiktarNo { get; set; }
        public double? sth_FormulMiktar { get; set; }
        public short? sth_eirs_senaryo { get; set; }
        public short? sth_eirs_tipi { get; set; }
        public DateTime? sth_teslim_tarihi { get; set; }
        public bool? sth_matbu_fl { get; set; }

        public string sth_eticaret_kanal_kodu { get; set; }
        public double? sth_satis_fiyat_doviz_cinsi { get; set; }
        public double? sth_satis_fiyat_doviz_kuru { get; set; }

       
        private void Temizle()
        {
            sth_tip = 2;
            sth_cins = 6;
            sth_evraktip = 2;
            sth_DBCno = 0;
            sth_SpecRECno = 0;
            sth_iptal = false;
            sth_fileid = 16;
            sth_hidden = false;
            sth_kilitli = false;
            sth_degisti = false;
            sth_checksum = 0;
            sth_create_user = 999;
            sth_lastup_user = 999;
            sth_special1 = "";
            sth_special2 = "";
            sth_special3 = "";
            sth_firmano = 0;
            sth_subeno = 0;
            sth_normal_iade = 0;
            sth_isk_mas1 = 0;
            sth_isk_mas2 = 1;
            sth_isk_mas3 = 1;
            sth_isk_mas4 = 1;
            sth_isk_mas5 = 1;
            sth_isk_mas6 = 1;
            sth_isk_mas7 = 1;
            sth_isk_mas8 = 1;
            sth_isk_mas9 = 1;
            sth_isk_mas10 = 1;
            sth_sat_iskmas1 = false;
            sth_sat_iskmas2 = false;
            sth_sat_iskmas3 = false;
            sth_sat_iskmas4 = false;
            sth_sat_iskmas5 = false;
            sth_sat_iskmas6 = false;
            sth_sat_iskmas7 = false;
            sth_sat_iskmas8 = false;
            sth_sat_iskmas9 = false;
            sth_sat_iskmas10 = false;
            sth_pos_satis = 0;
            sth_promosyon_fl = false;
            sth_cari_cinsi = 0;
            sth_cari_kodu = "";
            sth_cari_grup_no = 0;
            sth_isemri_gider_kodu = "";
            sth_plasiyer_kodu = "";
            sth_har_doviz_cinsi = 0;
            sth_har_doviz_kuru = 1;
            sth_alt_doviz_kuru = 1;
            sth_stok_doviz_cinsi = 0;
            sth_stok_doviz_kuru = 1;
            sth_tutar = 0;
            sth_iskonto1 = 0;
            sth_iskonto2 = 0;
            sth_iskonto3 = 0;
            sth_iskonto4 = 0;
            sth_iskonto5 = 0;
            sth_iskonto6 = 0;
            sth_masraf1 = 0;
            sth_masraf2 = 0;
            sth_masraf3 = 0;
            sth_masraf4 = 0;
            sth_vergi_pntr = 0;
            sth_vergi = 0;
            sth_masraf_vergi_pntr = 0;
            sth_masraf_vergi = 0;
            sth_netagirlik = 0;
            sth_odeme_op = 0;
            sth_sip_uid = Guid.Empty;
            sth_fat_uid = Guid.Empty;
            sth_cari_srm_merkezi = "";
            sth_stok_srm_merkezi = "";
            sth_fis_tarihi = Convert.ToDateTime("1899-12-30 00:00:00.000");
            sth_fis_sirano = 0;
            sth_vergisiz_fl = false;
            sth_maliyet_ana = 0;
            sth_maliyet_alternatif = 0;
            sth_maliyet_orjinal = 0;
            sth_adres_no = 0;
            sth_parti_kodu = "";
            sth_lot_no = 0;
            sth_kons_uid = Guid.Empty;
            sth_proje_kodu = "";
            sth_exim_kodu = "";
            sth_otv_pntr = 0;
            sth_otv_vergi = 0;
            sth_brutagirlik = 0;
            sth_disticaret_turu = 0;
            sth_otvtutari = 0;
            sth_otvvergisiz_fl = false;
            sth_oiv_pntr = 0;
            sth_oiv_vergi = 0;
            sth_oivvergisiz_fl = false;
            sth_fiyat_liste_no = 1;
            sth_oivtutari = 0;
            sth_Tevkifat_turu = 0;
            sth_nakliyedeposu = 0;
            sth_nakliyedurumu = 0;
            sth_yetkili_uid = Guid.Empty;
            sth_taxfree_fl = false;
            sth_ilave_edilecek_kdv = 0;
            sth_ismerkezi_kodu = "";
            sth_HareketGrupKodu1 = "";
            sth_HareketGrupKodu2 = "";
            sth_HareketGrupKodu3 = "";
            sth_Olcu1 = 0;
            sth_Olcu2 = 0;
            sth_Olcu3 = 0;
            sth_Olcu4 = 0;
            sth_Olcu5 = 0;
            sth_FormulMiktarNo = 0;
            sth_FormulMiktar = 0;
            sth_eirs_senaryo = 0;
            sth_eirs_tipi = 0;
            sth_teslim_tarihi = Convert.ToDateTime("1899-12-30");
            sth_eticaret_kanal_kodu = "";
            sth_matbu_fl = false;
            sth_satis_fiyat_doviz_cinsi = 0;
            sth_satis_fiyat_doviz_kuru = 1;

        }

        [Ignore] public string Renk { get; set; }
        [Ignore] public string Beden { get; set; }
        [Ignore] public DateTime? UretimTarihi { get; set; }
        [Ignore] public DateTime? SonKullanmaTarihi { get; set; }
    }
}