using DevExpress.XtraEditors.Controls;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Core.Result;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.Receteler;
using My.Entities.UretimStoklar;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using MyUI.ReceteModule;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUI.HizliUretimModule {
    public partial class FrmHizliUretimEG : MyFrmKayit {
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private IMikroGenelService _srvGenelMikro = Ortak.DbMikro.GenelServis;
        private IMikroStokService _mikroStokService = Ortak.DbMikro.Stoklar;
        private MikroKayitManager _mngMikroKayit;
        private MikroConvertManager _mngConvert;
        private ReceteManager _mngRecete;
        List<MikroDepo> lisDepoGiris = new List<MikroDepo>();
    
        //  private ReceteKayitModel _mdlRecete;
        Guid? rcaId = Guid.Empty;
        public FrmHizliUretimEG() {
            InitializeComponent();
        }

        private void FrmHizliUretimEG_Load(object sender, EventArgs e) {
            var PlGizle = !Ortak.PlKapat;
            TxtPartiNo.Visible = PlGizle;
            LblPartiNo.Visible = PlGizle;
            TxtLotNo.Visible = PlGizle;
            LblLotNo.Visible = PlGizle;
            TxtStokKodu.ButtonClick += TxtVarsayilanStokKodu_ButtonClick;
            TxtStokAdi.ButtonClick += TxtVarsayilanStokAdi_ButtonClick;
            //TxtCariKodu.Leave += TxtCariKodu_Leave;
            //TxtCariUnvani.Leave += TxtCariUnvani_Leave;
            TxtDepoNoGiris.Popup += TxtDepoNoGiris_Popup;
           
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro, Ortak.DbMikro);
            _mngConvert = new MikroConvertManager(Ortak.DbPro, Ortak.DbMikro);
            _mngRecete = new ReceteManager(Ortak.DbPro);

            BaglaDepolar();

            string modul = "MikroEntegre";
            string grubu = MikroAyarFisTurleri.HizliUretimFisi.ToString();
            int depoNo1 = -1;
           
            var mikroEntAyarlar = Ortak.MikroEntAyarlar.Where(c => c.Modul == "MikroEntegre").ToList();
            var dpkd1 = mikroEntAyarlar.Find(c => c.Modul == modul && c.Grup == grubu && c.Kodu == "GirisDepoKodu");
         
            if (dpkd1 != null && !string.IsNullOrEmpty(dpkd1.Deger)) depoNo1 = Convert.ToInt32(dpkd1.Deger);
           
            var f1 = lisDepoGiris.Find(c => c.DepoNo == depoNo1);
            if (f1 != null) {
                TxtDepoNoGiris.EditValue = f1.DepoNo;
                TxtDepoNoGiris.SelectedText = f1.DepoAdi;
                 
            }
           
            AcilisBittimi = true;
        }

        private void TxtVarsayilanStokKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            FrmReceteListesi f = new FrmReceteListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi) {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                TxtStokAdi.Text = st.ReceteAdi;
                TxtStokKodu.Text = st.ReceteKodu;
                rcaId = st.Id; 
                TxtBirim1.Text = st.EntegreBirim;
                //RenkBagla(st.StokKodu);
                //BedenBagla(st.StokKodu);
            }
        }
        private void TxtVarsayilanStokAdi_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            FrmReceteListesi f = new FrmReceteListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi) {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                TxtStokAdi.Text = st.ReceteAdi;
                TxtStokKodu.Text = st.ReceteKodu;
                rcaId = st.Id;
                TxtBirim1.Text = st.EntegreBirim;
            }
        }
        public void BaglaDepolar() {

            var rs = _srvGenelMikro.GetMikroDepoListesi(" order by dep_no");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            foreach (var itm in rs.Data) {
                lisDepoGiris.Add(itm.Clone());               
            }
            bsDpGir.DataSource = lisDepoGiris;           
            TxtDepoNoGiris.Properties.SearchMode = SearchMode.AutoSuggest;         
            TxtDepoNoGiris.Properties.DataSource = bsDpGir;
            TxtDepoNoGiris.Properties.DisplayMember = "DepoAdi";
            TxtDepoNoGiris.Properties.ValueMember = "DepoNo";
            TxtDepoNoGiris.Refresh();            
            TxtDepoNoGiris.Properties.ForceInitialize();           
        }
        private void TxtDepoNoGiris_Popup(object sender, EventArgs e) {
            TxtDepoNoGiris.Properties.ForceInitialize();
        }
      
        //public void RenkBagla(string stokkodu) { 
        //    try { var rs = _mikroStokService.GetRenkByStokKodu(stokkodu);
        //        if (!rs.Success) {  MesajHata(rs.Message);  return;   }  var rnk = rs.Data;
        //        if (rnk != null) {   CmbRenk.Properties.Items.Clear();   CmbRenk.MyDataBagla(rnk.GetRenkler());   }
        //    } catch {  } }
        //public void BedenBagla(string stokkodu) { 
        //    try { var rs = _mikroStokService.GetBedenByStokKodu(stokkodu);
        //         if (!rs.Success) {  MesajHata(rs.Message);  return;   } var bdn = rs.Data;
        //        if (bdn != null) {  CmbBeden.Properties.Items.Clear(); CmbBeden.MyDataBagla(bdn.GetBedenler());  }
        //    } catch { } }

        private void Kaydet() {


            if (TxtMiktar.MyDegerDouble()<=0) {
                MesajHata("Lütfen Miktar Giriniz.");
                return;
            }

            var rsrct = _mngRecete.GetReceteKayit(rcaId);
            if (!rsrct.Success) {
                MesajHata(rsrct.Message);
                return;
            }
            var recete = rsrct.Data;

            var MikroEntAyarlar = Ortak.MikroEntAyarlar.Where(c => c.Modul == "MikroEntegre").ToList();
            MikroFisGirisTurleri HizliUretimFisiTuru = MikroFisGirisTurleri.StokVirmanFisi;
            var ft_USFCFT = MikroEntAyarlar.Find(c => c.Modul == "MikroEntegre" && c.Grup == "FisTuru" && c.Kodu == "HizliUretimFisiTuru");
            if (ft_USFCFT != null) {
                HizliUretimFisiTuru = MikroKayitFisTurleri.GetHizliUretimFisiTuru(ft_USFCFT.Deger);
            }
            List<StokHareketleriModel> _lisStokFisi = new List<StokHareketleriModel>();
            List<MikroStokHareketleri> _lisMikro = new List<MikroStokHareketleri>();

            double miktar = TxtMiktar.MyDegerDouble();
            string stokKodu = TxtStokKodu.Text;
            string parti = TxtPartiNo.Text;
            int depoGir = -1;
            
            var mdlGir = (MikroDepo)TxtDepoNoGiris.GetSelectedDataRow();
            if (mdlGir != null) depoGir = mdlGir.DepoNo;
            
            int lot = TxtLotNo.Text.ToInt32();
            var sth = new StokHareketleriModel() {
                sth_Guid = Guid.NewGuid(),
                sth_aciklama = "HizliUretimGiris",
                sth_belge_no = "",
                sth_belge_tarih = DateTime.Now,
                sth_birim_pntr = 1,
                sth_malkbl_sevk_tarihi = DateTime.Now,
                sth_miktar = miktar,
                sth_miktar2 = miktar,
                sth_stok_kod = stokKodu,
                sth_tarih = DateTime.Now,
                sth_parti_kodu = parti,
                sth_lot_no = lot,
                sth_tip = 0,
                sth_special2 = "HZL",
                
            };
            sth = _mngConvert.SetHizliUretimFisiUrunGirisAyar(sth, MikroEntAyarlar, depoGir);
            _lisStokFisi.Add(sth);

            double gec_miktar = 0.0;
            double fire_miktar = 0.0;
            gec_miktar = miktar;
            foreach (var itm in recete.ReceteDetaylar) {
                gec_miktar = 0.0;
                fire_miktar = 0.0;
                gec_miktar = itm.Miktar;
                if (itm.FireYuzde != 0) {
                    fire_miktar = gec_miktar * (itm.FireYuzde / 100);
                }
                gec_miktar = gec_miktar + fire_miktar;
                var sth2 = new StokHareketleriModel() {
                    sth_Guid = Guid.NewGuid(),
                    sth_aciklama = "HizliUretimStokCikis",
                    sth_belge_no = "",
                    sth_belge_tarih = DateTime.Now,
                    sth_birim_pntr = 1,
                    sth_malkbl_sevk_tarihi = DateTime.Now,
                    sth_miktar = gec_miktar * miktar,
                    sth_miktar2 = gec_miktar * miktar,
                    sth_stok_kod = itm.VarsayilanStokKodu,
                    sth_tarih = DateTime.Now,
                    sth_parti_kodu = "",
                    sth_lot_no = 0,
                    sth_tip = 1,
                    sth_special2 = "HZL"
                };
                sth2 = _mngConvert.SetHizliUretimFisiStokCikisAyar(sth2, MikroEntAyarlar );
                _lisStokFisi.Add(sth2);
            }

            if (HizliUretimFisiTuru == MikroFisGirisTurleri.StokVirmanFisi) {
                var rs1 = _mngConvert.ConvertStokVirmanFisi(_lisStokFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                }
                _lisMikro.AddRange(rs1.Data);
            }
            else if (HizliUretimFisiTuru == MikroFisGirisTurleri.UretimHareketFisi) {
                var rs1 = _mngConvert.ConvertUretimHareketFisi(_lisStokFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                }
                _lisMikro.AddRange(rs1.Data);
            }
            else {
                var rs1 = _mngConvert.ConvertStokVirmanFisi(_lisStokFisi, Ortak.MikroEntAyarlar);
                if (!rs1.Success) {
                    MesajHata(rs1.Message);
                    return;
                }
                _lisMikro.AddRange(rs1.Data);
            }
            var rsKyt = _mngMikroKayit.StokHareketKaydet(_lisMikro);
            if (!rsKyt.Success) {
                MesajHata(rsKyt.Message);
                return;
            }
            else {

                //string seri = _lisMikro[0].sth_evrakno_seri;
                //string sira = _lisMikro[0].sth_evrakno_sira.ToString();
                //string sonseri = _lisMikro[_lisMikro.Count - 1].sth_evrakno_seri.ToString();
                //string sonsira = _lisMikro[_lisMikro.Count - 1].sth_evrakno_sira.ToString();
                //var son = _mngSip.SiparisEntGuncelle(_mdl.Siparis.Id, seri, sira, sonseri, sonsira);
                //if (!son.Success) {
                //    MesajHata(son.Message);
                //    return;
                //}

                MesajBilgi("KayıtEdildi");
                BtnKaydet.Enabled = false;
            }
        }

        private void BtnKaydet_Click(object sender, EventArgs e) {
            Kaydet();
        }
    }
}
