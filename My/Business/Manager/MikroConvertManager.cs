using My.Core.Result;
using My.Core;
using My.Entities.Ayarlar;
using My.Entities.Mikro;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace My.Business.Manager {
    public class MikroConvertManager {


        private readonly DatabaseFactoryMikro _dbMikro;
        private DatabaseFactoryPro _dbPro;
        public MikroConvertManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro) {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
        }

        public IDataResult<int> GetStokHareketEvrakSira(string seri, short sth_tip, short sth_cins, short sth_evraktip) {
            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var evrak = 0;
                var evrakNo = 0;
                var qry = $"SELECT ISNULL(MAX(sth_evrakno_sira),0) as [evrakno] FROM STOK_HAREKETLERI WHERE sth_tip={sth_tip}   " +
                    $" AND sth_cins={sth_cins} AND sth_evraktip={sth_evraktip} AND sth_evrakno_seri='" + seri.Trim() + "'";
                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                var objReader = cmd.ExecuteReader();
                while (objReader.Read()) evrak = Convert.ToInt32(objReader["evrakno"].ToString());
                con.Close();
                evrakNo = evrak + 1;
                return new SuccessDataResult<int>(evrakNo);
            } catch (Exception e) {
                return new ErrorDataResult<int>(e.Message);
            }
        }
        public IDataResult<int> GetStokVirmanEvrakSira(string seri, short sth_cins = 3, short sth_evraktip = 6) {
            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var evrak = 0;
                var evrakNo = 0;
                var qry = $"SELECT ISNULL(MAX(sth_evrakno_sira),0) as [evrakno] FROM STOK_HAREKETLERI WHERE    " +
                    $"  sth_cins={sth_cins} AND sth_evraktip={sth_evraktip} AND sth_evrakno_seri='" + seri.Trim() + "'";
                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                var objReader = cmd.ExecuteReader();
                while (objReader.Read()) evrak = Convert.ToInt32(objReader["evrakno"].ToString());
                con.Close();
                evrakNo = evrak + 1;
                return new SuccessDataResult<int>(evrakNo);
            } catch (Exception e) {
                return new ErrorDataResult<int>(e.Message);
            }
        }
        public IDataResult<int> GetUretimHareketFisiEvrakSira(string seri, short sth_cins = 7, short sth_evraktip = 7) {
            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var evrak = 0;
                var evrakNo = 0;
                var qry = $"SELECT ISNULL(MAX(sth_evrakno_sira),0) as [evrakno] FROM STOK_HAREKETLERI WHERE    " +
                    $"  sth_cins={sth_cins} AND sth_evraktip={sth_evraktip} AND sth_evrakno_seri='" + seri.Trim() + "'";
                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                var objReader = cmd.ExecuteReader();
                while (objReader.Read()) evrak = Convert.ToInt32(objReader["evrakno"].ToString());
                con.Close();
                evrakNo = evrak + 1;
                return new SuccessDataResult<int>(evrakNo);
            } catch (Exception e) {
                return new ErrorDataResult<int>(e.Message);
            }
        }

        #region SetAyar
        /*

    UretimUrunGirisFisi,
    UretimStokCikisFisi,

    UretimUrunFireGirisFisi,
    UretimStokFireCikisFisi,

    HizliUretimFisi ,

    SarfCikisFisi,
    FireGirisFisi,
    DepoSevkFisi,

     */

         
        public StokHareketleriModel SetUretimUrunGirisFisiAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.UretimUrunGirisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;
            string giderkodu = "";
            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");
            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;
            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = 0;
            mdl.sth_giris_depo_no = depoNo;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;


            return mdl;
        }
        public StokHareketleriModel SetUretimStokCikisFisiAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.UretimStokCikisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_giris_depo_no = 0;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;

            return mdl;
        }
        public StokHareketleriModel SetUretimUrunFireCikisFisiAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.UretimUrunFireCikisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_giris_depo_no = 0;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;

            return mdl;
        }
        public StokHareketleriModel SetUretimStokFireCikisFisiAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.UretimStokFireCikisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_giris_depo_no = 0;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;

            return mdl;
        } 
        public StokHareketleriModel SetSarfCikisFisiTuruAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.SarfCikisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_giris_depo_no = 0;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;
            return mdl;
        }
        public StokHareketleriModel SetFireGirisFisiTuruAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.FireGirisFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "DepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_giris_depo_no = 0;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;
            return mdl;
        }
        public StokHareketleriModel SetDepoSevkFisiAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar, string belgeNo) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.DepoSevkFisi.ToString();
            int firmaNo = 0;
            short userkod = 995;

            var firn = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
            var klnc = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
            if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
            if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

            string evrakseri = "";
            int depoNoGir = 0;
            int depoNoCik = 0;
            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkdGir = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GirisDepo");
            var dpkdCik = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "CikisDepo");
            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkdGir != null && !string.IsNullOrEmpty(dpkdGir.Deger)) depoNoGir = Convert.ToInt32(dpkdGir.Deger);
            if (dpkdCik != null && !string.IsNullOrEmpty(dpkdCik.Deger)) depoNoCik = Convert.ToInt32(dpkdCik.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_cikis_depo_no = depoNoCik;
            mdl.sth_giris_depo_no = depoNoGir;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;
            return mdl;
        }

        public StokHareketleriModel SetHizliUretimFisiUrunGirisAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar,int depokodu=-1) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.HizliUretimFisi.ToString();
            string evrakseri = "";
            int depoNo = 0;

            string giderkodu = "";

            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GirisDepoKodu");

            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GirisGiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            if (depokodu!=-1) {
                depoNo = depokodu;
            }

            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger);

            mdl.sth_giris_depo_no = depoNo;
            mdl.sth_cikis_depo_no = 0; 
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;


            return mdl;
        }
        public StokHareketleriModel SetHizliUretimFisiStokCikisAyar(StokHareketleriModel mdl, List<Ayar> mikroEntAyarlar, int depokodu = -1) {
            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.HizliUretimFisi.ToString();
            string evrakseri = "";
            int depoNo = 0; 
            string giderkodu = ""; 
            var seri = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "EvrakSeri");
            var dpkd = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "CikisDepoKodu"); 
            var gdrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "CikisGiderKodu");
            if (seri != null && !string.IsNullOrEmpty(seri.Deger)) evrakseri = Convert.ToString(seri.Deger);
            if (dpkd != null && !string.IsNullOrEmpty(dpkd.Deger)) depoNo = Convert.ToInt32(dpkd.Deger);
            if (gdrk != null) giderkodu = gdrk.Deger;

            if (depokodu != -1) {
                depoNo = depokodu;
            }


            string prjkodu = "";
            string srmmrkz = "";
            var prkod = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "ProjeKodu");
            var srmrk = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "SrmMerkezi");
            if (prkod != null && !string.IsNullOrEmpty(prkod.Deger)) prjkodu = Convert.ToString(prkod.Deger);
            if (srmrk != null && !string.IsNullOrEmpty(srmrk.Deger)) srmmrkz = Convert.ToString(srmrk.Deger); 

            mdl.sth_giris_depo_no = 0;
            mdl.sth_cikis_depo_no = depoNo;
            mdl.sth_evrakno_seri = evrakseri;
            mdl.sth_evrakno_sira = 0;
            mdl.sth_satirno = 0;
            mdl.sth_isemri_gider_kodu = giderkodu;
            mdl.sth_proje_kodu = prjkodu;
            mdl.sth_stok_srm_merkezi = srmmrkz;


            return mdl;
        }

        #endregion

        #region Convert
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tip">0 giriş,1 çıkış </param>
        /// <param name="_model"></param>
        /// <param name="mikroEntAyarlar"></param>
        /// <returns></returns>
        public IDataResult<List<MikroStokHareketleri>> ConvertStokVirmanFisi(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {

                // short sth_tip = tip; hareketten al
                short sth_cins = 3;
                short sth_evraktip = 6;
                short sth_normal_iade = 0; // 0:Normal 1:İade


                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokVirmanEvrakSira(seri.Trim());
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                int i = 0;
                var _list = new List<MikroStokHareketleri>();
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = mdl.sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    hrk.UretimTarihi = mdl.UretimTarihi;
                    hrk.SonKullanmaTarihi = mdl.SonKullanmaTarihi;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        public IDataResult<List<MikroStokHareketleri>> ConvertUretimHareketFisi(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {

                // short sth_tip = tip; hareketten al
                short sth_cins = 7;
                short sth_evraktip = 7;
                short sth_normal_iade = 0; // 0:Normal 1:İade


                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetUretimHareketFisiEvrakSira(seri.Trim());
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                int i = 0;
                var _list = new List<MikroStokHareketleri>();
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = mdl.sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    hrk.UretimTarihi = mdl.UretimTarihi;
                    hrk.SonKullanmaTarihi = mdl.SonKullanmaTarihi;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        public IDataResult<List<MikroStokHareketleri>> ConvertUretimdenGirisFisi(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {
                short sth_tip = 0;
                short sth_cins = 7;
                short sth_evraktip = 12;
                short sth_normal_iade = 0;

                int firmaNo = 0;
                short userkod = 999;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokHareketEvrakSira(seri.Trim(), sth_tip, sth_cins, sth_evraktip);
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                var _list = new List<MikroStokHareketleri>();
                var i = 0;
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    hrk.UretimTarihi = mdl.UretimTarihi;
                    hrk.SonKullanmaTarihi = mdl.SonKullanmaTarihi;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        public IDataResult<List<MikroStokHareketleri>> ConvertUretimeCikisFisi(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {
                short sth_tip = 1;
                short sth_cins = 7;
                short sth_evraktip = 0;
                short sth_normal_iade = 0; // 0:Normal 1:İade
                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokHareketEvrakSira(seri.Trim(), sth_tip, sth_cins, sth_evraktip);
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                var _list = new List<MikroStokHareketleri>();
                var i = 0;
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        public IDataResult<List<MikroStokHareketleri>> ConvertSayimDepoGiris(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {
                short sth_tip = 0;
                short sth_cins = 10;
                short sth_evraktip = 12;
                short sth_normal_iade = 0; // 0:Normal 1:İade
                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);
                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokHareketEvrakSira(seri.Trim(), sth_tip, sth_cins, sth_evraktip);
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                var _list = new List<MikroStokHareketleri>();
                var i = 0;
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    hrk.UretimTarihi = mdl.UretimTarihi;
                    hrk.SonKullanmaTarihi = mdl.SonKullanmaTarihi;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        public IDataResult<List<MikroStokHareketleri>> ConvertSarfDepoCikis(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {
                short sth_tip = 1;
                short sth_cins = 5;
                short sth_evraktip = 0;
                short sth_normal_iade = 0;

                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokHareketEvrakSira(seri.Trim(), sth_tip, sth_cins, sth_evraktip);
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                var _list = new List<MikroStokHareketleri>();
                var i = 0;
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }

        public IDataResult<List<MikroStokHareketleri>> ConvertFireCikis(List<StokHareketleriModel> _models, List<Ayar> mikroEntAyarlar, int evrakSirayaEkle = 0) {
            try {
                short sth_tip = 1;
                short sth_cins = 4;
                short sth_evraktip = 0;
                short sth_normal_iade = 0;

                int firmaNo = 0;
                short userkod = 995;
                var firn = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "FirmaNo");
                var klnc = mikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "GENEL" && c.Kodu == "KullaniciKodu");
                if (firn != null && !string.IsNullOrEmpty(firn.Deger)) firmaNo = Convert.ToInt32(firn.Deger);
                if (klnc != null && !string.IsNullOrEmpty(klnc.Deger)) userkod = Convert.ToInt16(klnc.Deger);

                string seri = _models.FirstOrDefault().sth_evrakno_seri;
                var rsevrakNo = GetStokHareketEvrakSira(seri.Trim(), sth_tip, sth_cins, sth_evraktip);
                if (!rsevrakNo.Success) {
                    return new ErrorDataResult<List<MikroStokHareketleri>>(rsevrakNo.Message);
                }
                var evrakNo = rsevrakNo.Data + evrakSirayaEkle;
                var _list = new List<MikroStokHareketleri>();
                var i = 0;
                foreach (var mdl in _models) {
                    if (mdl.sth_Guid == null || mdl.sth_Guid == Guid.Empty) {
                        mdl.sth_Guid = MyGuid.NewGuid();
                    }
                    var hrk = new MikroStokHareketleri(mdl.sth_Guid, seri, evrakNo, mdl.sth_belge_no,
                        mdl.sth_stok_kod, mdl.sth_aciklama, mdl.sth_miktar, mdl.sth_miktar2,
                        mdl.sth_giris_depo_no, mdl.sth_cikis_depo_no, mdl.sth_birim_pntr, i);
                    hrk.sth_tip = sth_tip;
                    hrk.sth_cins = sth_cins;
                    hrk.sth_evraktip = sth_evraktip;
                    hrk.sth_normal_iade = sth_normal_iade;
                    hrk.sth_firmano = firmaNo;
                    hrk.sth_tutar = mdl.Fiyat * mdl.sth_miktar;
                    hrk.sth_tarih = Convert.ToDateTime(Convert.ToDateTime(mdl.sth_tarih).ToShortDateString());
                    hrk.sth_belge_tarih = mdl.sth_belge_tarih;
                    hrk.sth_malkbl_sevk_tarihi = mdl.sth_malkbl_sevk_tarihi;
                    hrk.sth_isemri_gider_kodu = mdl.sth_isemri_gider_kodu;
                    hrk.sth_proje_kodu = mdl.sth_proje_kodu;
                    hrk.sth_stok_srm_merkezi = mdl.sth_stok_srm_merkezi;
                    hrk.sth_parti_kodu = mdl.sth_parti_kodu;
                    hrk.sth_lot_no = mdl.sth_lot_no;
                    hrk.sth_create_user = userkod;
                    hrk.sth_lastup_user = userkod;
                    hrk.sth_special1 = mdl.sth_special1;
                    hrk.sth_special2 = mdl.sth_special2;
                    hrk.sth_special3 = mdl.sth_special3;
                    if (hrk.sth_giris_depo_no <= 0) {
                        hrk.sth_giris_depo_no = 1;
                    }
                    if (hrk.sth_cikis_depo_no <= 0) {
                        hrk.sth_cikis_depo_no = 1;
                    }
                    hrk.Renk = mdl.Renk;
                    hrk.Beden = mdl.Beden;
                    _list.Add(hrk);
                    i++;
                }
                return new SuccessDataResult<List<MikroStokHareketleri>>(_list);
            } catch (Exception e) {
                return new ErrorDataResult<List<MikroStokHareketleri>>(e.Message);
            }
        }
        #endregion

    }
}
