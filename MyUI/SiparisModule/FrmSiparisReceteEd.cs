using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Templer;
using My.Core;
using My.Core.Result;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.ReceteGruplar;
using My.Entities.Receteler;
using My.Entities.Siparisler;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using MyUI.MikroModule;
using MyUI.MyControl;
using MyUI.ReceteModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.SiparisModule {
    public partial class FrmSiparisReceteEd : MyFrmKayitFull {
        private readonly ITempMikroStokService _srvTmpStk = Ortak.DbPro.TempMikroStok;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private MikroKayitManager _mngMikroKayit;
        private SiparisManager _mng;
        private ReceteManager _mngRecete;
        private ReceteGrupManager _mngReceteGrup;
        private SiparisKayitModel _mdl;
        private List<MikroStok> StoklarAll;
        private List<MikroStokRenk> StokRenklerAll;
        public System.Action ActionAktar;
        bool degistirilemez = false;
        public string Turu = "Recete";
        #region Form
        public FrmSiparisReceteEd() {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Eventler();
        }
        private void Eventler() {
            this.Load += Frm_Load;
            this.FormClosed += Frm_FormClosed;
            this.KeyDown += Frm_KeyDown;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            TxtSiparisKodu.ButtonClick += TxtSiparisKodu_ButtonClick;
        }
        private void Frm_Load(object sender, System.EventArgs e) {
            _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            _mngRecete = new ReceteManager(Ortak.DbPro);
            _mngReceteGrup = new ReceteGrupManager(Ortak.DbPro);
            _mngMikroKayit = new MikroKayitManager(Ortak.DbPro,Ortak.DbMikro);
            Bagla();
            StoklarBagla();
            if (!Ortak.LisansAktif) {
                BtnKaydet.Enabled = false;
                BtnSil.Enabled = false;
            }
            AcilisBittimi = true;
        }
        private void Frm_FormClosed(object sender, FormClosedEventArgs e) {
            PanelDetay.Controls.Clear();
        }
        private void Frm_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F5) BtnReceteGrupSec.PerformClick();
            else if (e.KeyCode == Keys.F6) BtnReceteTekSec.PerformClick();
            else if (e.KeyCode == Keys.F7) BtnReceteDegistir.PerformClick();
            else if (e.KeyCode == Keys.F8) BtnReceteSil.PerformClick();
        }
        private void StoklarBagla() {
            var rsStA = Ortak.DbMikro.Stoklar.GetViewListWhere("", Ortak.MikroStokGrubu);
            if (!rsStA.Success) {
                MesajHata(rsStA.Message);
                return;
            }
            StoklarAll = rsStA.Data.ToList();
            /*************/
            var rsStR = Ortak.DbMikro.Stoklar.GetRenkListWhere("");
            if (!rsStR.Success) {
                MesajHata(rsStR.Message);
                return;
            }
            StokRenklerAll = rsStR.Data.ToList();
        }
        private void Bagla() {
            if (_mng == null) {
                _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            }
            if (IdGuid.IsNullOrEmpty()) {
                YeniKayit = true;
                _mdl = _mng.GetSiparis();
                TemizleText();
            }
            else {
                YeniKayit = false;
                var rs = _mng.GetSiparis(IdGuid);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }
                _mdl = rs.Data;
                AktarTextlere();
                if (SiparisDegistirilemez(IdGuid)) {
                    tableLayoutPanel1.Enabled = false;
                    BtnSil.Enabled = false;
                    myView1.SutunEditKapa(nameof(SiparisHareket.Miktar));
                    myView1.SutunEditKapa(nameof(SiparisHareket.Aciklama));
                    myView1.SutunEditKapa(nameof(SiparisHareket.EtiketAciklama));
                    myView1.SutunEditKapa(nameof(SiparisHareket.Parti));
                    myView1.SutunEditKapa(nameof(SiparisHareket.Lot));
                    myView1.SutunEditKapa(nameof(SiparisHareket.Beden));
                    degistirilemez = true;
                }
            }
        }
        private bool SiparisDegistirilemez(Guid? sipid) {
            var rs = _mng.SipariseBagliUretimVarmi(sipid);
            if (rs.Success) {
                if (rs.Data) {
                    MesajBilgi("Reçetenin üretimi başlatılmış değişiklik yapabilmek için üretimi silmeniz gerekir.");
                    return true;
                }

                return false;
            }
            return true;
        }
        private void GridBagla() {
            myGrid1.DataSource = null;
            myGrid1.DataSource = _mdl.Hareketler;
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }
        private void GridBaglaAcilis() {
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            myView1.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Aciklama"].OptionsColumn.AllowEdit = true;
          
            myView1.Columns["Parti"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Lot"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Aciklama"].AppearanceHeader.BackColor = Color.Green;
          
            myView1.Columns["Miktar"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Parti"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Lot"].AppearanceHeader.BackColor = Color.Green;

            myView1.SutunEditAc(nameof(SiparisHareket.EtiketAciklama));
            myView1.SutunCaptionColor(nameof(SiparisHareket.EtiketAciklama), Color.Green); 

            myView1.SutunEditAc(nameof(SiparisHareket.Beden));
            myView1.SutunCaptionColor(nameof(SiparisHareket.Beden), Color.Green);
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }

            myView1.SutunFormat("Miktar", DevExpress.Utils.FormatType.Numeric, "N0");
        }
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("SipId");
            myView1.SutunGizle("RcAId");
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }
        private void EvrakNoAl() {
            var rs = _srvGenel.GetEvrakNo("Uretim");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            TxtSiparisKodu.Text = rs.Data;
        }
        private void AktarTextlere() {
            Turu = _mdl.Siparis.Turu;
            TxtSiparisKodu.Text = _mdl.Siparis.SiparisKodu;
            TxtAciklama.Text = _mdl.Siparis.Aciklama;
            TxtTarih.Text = _mdl.Siparis.Tarih.ToString();
            TxtTeslimTarihi.Text = _mdl.Siparis.TeslimTarihi.ToString();
            ChcKapandi.IsOn = _mdl.Siparis.Kapandi;
            TxtCariKodu.Text = _mdl.Siparis.CariKodu;
            TxtCariUnvani.Text = _mdl.Siparis.CariUnvani;
            GridBagla();
            GridBaglaAcilis();
        }
        private void AktarModele() {
            _mdl.Siparis.Turu = Turu;
            _mdl.Siparis.SiparisKodu = TxtSiparisKodu.Text;
            _mdl.Siparis.Aciklama = TxtAciklama.Text;
            if (!TxtTarih.Text.IsNullOrEmpty()) {
                _mdl.Siparis.Tarih = Convert.ToDateTime(TxtTarih.Text);
            }
            else {
                _mdl.Siparis.Tarih = null;
            }
            if (!TxtTarih.Text.IsNullOrEmpty()) {
                _mdl.Siparis.TeslimTarihi = Convert.ToDateTime(TxtTeslimTarihi.Text);
            }
            else {
                _mdl.Siparis.TeslimTarihi = null;
            }
            _mdl.Siparis.Kapandi = ChcKapandi.IsOn;
            if (string.IsNullOrEmpty(_mdl.Siparis.Durumu)) {
                _mdl.Siparis.Durumu = "YeniKayit";
            }
            double adet = 0;
            string notu = "";
            foreach (var itm in _mdl.Hareketler) {
                itm.SipId = _mdl.Siparis.Id;

                adet += itm.Miktar;
                notu += "-" + itm.ReceteAdi;
            }
            _mdl.Siparis.Miktar = adet;
            _mdl.Siparis.Notu = notu;
            foreach (var itm in _mdl.Detaylar) {
                itm.SipId = _mdl.Siparis.Id;

            }
            _mdl.Siparis.CariKodu = TxtCariKodu.Text;
            _mdl.Siparis.CariUnvani = TxtCariUnvani.Text;


        }
        private void TemizleText() {
            TxtSiparisKodu.Text = "";
            TxtAciklama.Text = "";
            TxtCariKodu.Text = "";
            TxtCariUnvani.Text = "";
            TxtTarih.Text = DateTime.Now.ToString();
            TxtTeslimTarihi.Text = DateTime.Now.ToString();
            ChcKapandi.IsOn = false;
            GridBagla();
            GridBaglaAcilis();
        }
        private bool TextLeriKontrolEt() {
            foreach (var itm in _mdl.Hareketler) {
                if (itm.YeniKayit) {
                    MesajBilgi("Üretimde Ayarlanmamış Kayıtlar var lütfen Yeni Kayıtları ayarlayınız");
                    return false;
                }
            }
            if (string.IsNullOrEmpty(TxtSiparisKodu.Text)) {
                EvrakNoAl();
            }
            if (string.IsNullOrEmpty(TxtSiparisKodu.Text)) {
                MesajHata("Lütfen Üretim kodunu giriniz");
                return false;
            }
            if (_mdl.Hareketler.Count <= 0) {
                MesajHata("Hareket Bulunamadı Kayıt Edilemez.");
                return false;
            }
            return true;
        }
        private void Kaydet() {
            if (!TextLeriKontrolEt()) {
                return;
            }
            AktarModele();
            var rs = _mng.SiparisKaydet(_mdl, YeniKayit);
            if (rs.Success) {
                KayitEdildi = true;
                ActionAktar?.Invoke();
                this.Close();
            }
            else {
                MesajHata(rs.Message);
            }
        }
        private void Sil() {
            if (!MesajSor("Kaydı silmek istiyormusunuz..")) {
                return;
            }
            var rs = _mng.SiparisSil(_mdl);
            if (rs.Success) {
                ActionAktar?.Invoke();
                KayitEdildi = true;
                this.Close();
            }
            else {
                MesajHata(rs.Message);
            }
        }
        private void TxtSiparisKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            if (!string.IsNullOrEmpty(TxtSiparisKodu.Text)) {
                if (!MesajSor("Sipariş Kodunu Değiştirmek istiyormusunuz")) {
                    return;
                }
            }
            EvrakNoAl();
        }

        private void BtnKaydet_Click(object sender, EventArgs e) {
            _mdl.Hareketler.ToList();
            //  TakipTip =1   {  /*PARTİ TAKİPLİ ÜRÜN*/}  TakipTip   =2 { /*PARTİ VE LOT TAKİPLİ ÜRÜN*/ } TakipTip   =3 {/*SERİ NOLU TAKİP*/ } 
            //  RbTakipTip =1 {  /*RENK  TAKİPLİ ÜRÜN*/}  RbTakipTip =2 { /*BEDEN TAKİPLİ ÜRÜN*/  }       RbTakipTip =3 { /*RENK VE BEDEN TAKİPLİ ÜRÜN*/}   
            //  üretim emri partilot varsa partilot girme  zorunlu olacak

            foreach (var itm in _mdl.Hareketler) {
                if (Ortak.PlKapat) {
                    continue;
                }
                var rsknt = _srvTmpStk.SelectFirst(c => c.StokKodu == itm.StokKodu, "*");
                if (rsknt.IsError) {
                    MesajHata(rsknt.Message);
                    return;
                }
                if (!Ortak.PlKapat)
                {
                    if (itm.Parti.IsNullOrEmpty())
                    { 
                        if (rsknt.Data.TakipTip == 1 || rsknt.Data.TakipTip == 2)
                        {
                            MesajBilgi(itm.StokKodu + " " + itm.StokAdi + " Üründe parti takibi var lütfen parti no giriniz. ");
                            return;
                        }
                    }
                } 
               if (itm.Beden.IsNullOrEmpty()) {
                   
                    if (rsknt.Data.RbTakipTip == 2 || rsknt.Data.TakipTip == 3) {
                        MesajBilgi(itm.StokKodu + " " + itm.StokAdi + " Üründe beden takibi var lütfen beden giriniz. ");
                        return;
                    }
                }
                if (itm.Lot.IsNullOrEmpty()|| itm.Lot=="0") {
                    itm.Lot="1";
                }
                var rsss = _mngMikroKayit. PartiLotDataOnceGirilmismi(itm.StokKodu, itm.Parti,Convert.ToInt32( itm.Lot));
                if (rsss.IsError) {
                    MesajBilgi(itm.StokKodu + " " + itm.StokAdi + " Üründe parti no kayıtlı lütfen yeni parti no giriniz. ");
                    return  ;
                }
            }
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e) {
            var rs = _mng.SiparisSilKontrol(_mdl.Siparis.Id);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            Sil();
        }
        private void BtnYazdir_Click(object sender, EventArgs e) {
            Yazdir();
        }
        private void BtnYazdirTek_Click(object sender, EventArgs e) {
            YazdirTek();
        }
        private void Yazdir() {
            const string YazdirmaAdi = "Siparis";
            DataSet ds = new DataSet("SiparisDS");
            ds.Tables.Add(_mdl.Siparis.ToDataTable("Siparis"));
            ds.Tables.Add(_mdl.Hareketler.ToDataTable("Hareketler"));
            ds.Tables.Add(_mdl.Detaylar.ToDataTable("Detaylar"));
            ds.Yaz(YazdirmaAdi, false);
        }
        private void YazdirTek() {
            const string YazdirmaAdi = "Siparis";
            YaziciAyar ayar = null;
            foreach (var itm in _mdl.Hareketler) {
                DataSet ds = new DataSet("SiparisDS");
                ds.Tables.Add(_mdl.Siparis.ToDataTable("Siparis"));
                ds.Tables.Add(_mdl.Hareketler.Where(c => c.Id == itm.Id).ToList()
                    .ToDataTable("Hareketler"));
                ds.Tables.Add(_mdl.Detaylar.Where(c => c.SipHId == itm.Id).ToList().ToDataTable("Detaylar"));
                if (ayar == null) ayar = ds.YaziciAyarAl(YazdirmaAdi); // ayar al
                if (ayar == null) break; // ayar alinanamadiysa çık
                if (ayar.YazdirmaTuru == YazdirmaTuru.Iptal || ayar.YazdirmaTuru == YazdirmaTuru.Dizayn) {
                    // yazdirma iptal edildiyse yada dizayn acildiysa yazdirma iptal
                    break;
                }
                else if (ayar.YazdirmaTuru == YazdirmaTuru.Onizle) {
                    for (int i = 0; i < ayar.Adet; i++) YazdirDevexp.Yazdir_Yolile(ds, false, true, ayar.YaziciAdi, ayar.DizaynYol);
                }
                else // yazdir
                {
                    for (int i = 0; i < ayar.Adet; i++) YazdirDevexp.Yazdir_Yolile(ds, false, false, ayar.YaziciAdi, ayar.DizaynYol);
                }
            }
        }
        #endregion
        /* ***********************/
        /* ***********************/
        private void BtnReceteDegistir_Click(object sender, EventArgs e) {
            ReceteDegistir();
            // RecetePanelBagla();
        }
        private void BtnReceteSil_Click(object sender, EventArgs e) {
            ReceteSil();
        }
        private void BtnReceteGrupSec_Click(object sender, EventArgs e) {
            ReceteGrupSec();
        }
        private void BtnReceteSec_Click(object sender, EventArgs e) {
            //   ReceteSec();
            ReceteTekSec();
        }
        private void ReceteSil() {
            if (!MesajSor("Kaydı silmek istiyormusunuz..")) {
                return;
            }
            var data = myView1.MyGetCurrentItem<SiparisHareket>();
            if (data == null) {
                return;
            }
            _mdl.Hareketler.Remove(data);
            List<SiparisHareketDetay> sto = new List<SiparisHareketDetay>();
            var stl = _mdl.Detaylar.Where(c => c.SipHId != data.Id);
            foreach (var itm in stl) {
                sto.Add(itm.Clone());
            }
            _mdl.Detaylar.Clear();
            _mdl.Detaylar.InsertRange(0, sto);
            GridBagla();
            PanelDetay.Controls.Clear();
        }
        private void RecetePanelBagla() {
            var data = myView1.MyGetCurrentItem<SiparisHareket>();
            if (data == null) return;
            List<SiparisHareketDetay> detaylar = new List<SiparisHareketDetay>();
            var detx = _mdl.Detaylar.Where(c => c.SipHId == data.Id).ToList();
            detaylar.InsertRange(0, detx);
            var rec = _mngRecete.GetReceteKayit(data.RcAId);
            if (!rec.Success) {
                MesajHata(rec.Message);
                return;
            }
            bool detaygoster = false;
            foreach (var itm in rec.Data.ReceteDetaylar) {
                if (!itm.SiparisdeGosterme) {
                    detaygoster = true;
                    break;
                }
            }
            if (!detaygoster) {
                return;
            }
            SiparisPanelControl f = new SiparisPanelControl {
                Visible = false,
                Dock = DockStyle.Fill,
                Hareket = data,
                Detaylar = detaylar,
                YeniKayit = data.YeniKayit,
                Recete = rec.Data,
                Size = PanelDetay.Size,
                KayitAction = PanelGeriAktar,
                StoklarAll = StoklarAll,
                StokRenklerAll = StokRenklerAll
            };
            PanelDetay.Controls.Clear();
            PanelDetay.Controls.Add(f);
            LblKoduAdi.Text = data.ReceteKodu + " - " + data.ReceteAdi;
            f.Visible = true;
        }
        private void PanelGeriAktar() {
            var sira = myView1.FocusedRowHandle;
            SiparisPanelControl f = PanelDetay.Controls[0] as SiparisPanelControl;
            var donen = f.Hareket.Clone();
            foreach (var hareket in _mdl.Hareketler) {
                if (hareket.Id == donen.Id) {
                    hareket.ReceteKodu = donen.ReceteKodu;
                    hareket.ReceteAdi = donen.ReceteAdi;
                    hareket.RcAId = donen.RcAId;
                    hareket.StokKodu = donen.StokKodu;
                    hareket.StokAdi = donen.StokAdi;
                    hareket.Renk = donen.Renk;
                    hareket.Beden = donen.Beden;
                    hareket.Birim = donen.Birim;
                    hareket.YeniKayit = false;
                    break;
                }
            }
            foreach (var itm in f.Detaylar) {
                foreach (var det in _mdl.Detaylar) {
                    if (det.SipHId == itm.SipHId && det.RcAId == itm.RcAId && det.RcDId == itm.RcDId) {
                        det.Cinsi = itm.Cinsi;
                        det.StokKodu = itm.StokKodu;
                        det.StokAdi = itm.StokAdi;
                        det.Birim = itm.Birim;
                        det.Renk = itm.Renk;
                        det.Beden = itm.Beden;
                        det.Miktar = itm.Miktar;
                        det.Aciklama = itm.Aciklama;
                        det.RcAId = itm.RcAId;
                        det.RcDId = itm.RcDId;
                        break;
                    }
                }
            }
            GridBagla();
            myView1.FocusedRowHandle = sira;
            PanelDetay.Controls.Clear();
            LblKoduAdi.Text = "..";
        }
        private void ReceteTekSec() {
            FrmReceteSec f = new FrmReceteSec { SecimIcinAcildi = true, WindowState = FormWindowState.Maximized };
            f.ShowDialog();
            if (f.Secildi) {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                var rec = _mngRecete.GetReceteKayit(st.Id);
                if (!rec.Success) {
                    MesajHata(rec.Message);
                    return;
                }
                var hareket = new SiparisHareket();
                var Detaylar = new List<SiparisHareketDetay>();
                var Recete = rec.Data;
                hareket.ReceteGrupKodu = "";
                hareket.ReceteKodu = Recete.Recete.ReceteKodu;
                hareket.ReceteAdi = Recete.Recete.ReceteAdi;
                hareket.RcAId = Recete.Recete.Id;
                hareket.Miktar = 1;

                hareket.StokKodu = Recete.Recete.EntegreStokKodu;
                hareket.StokAdi = Recete.Recete.EntegreStokAdi;
                hareket.Birim = Recete.Recete.EntegreBirim;
                bool ilk = true;
                SiparisHareketDetay det;
                foreach (ReceteDetay itm in Recete.ReceteDetaylar) {
                    if (ilk) {
                        ilk = false;
                        hareket.Renk = itm.Renk;
                        hareket.Beden = itm.Beden;
                    }
                    if (!itm.SiparisdeGosterme) // siparişde göster varsa yeni kayıt olsun
                    {
                        hareket.YeniKayit = true;
                    }
                    det = new SiparisHareketDetay {
                        Id = MyGuid.NewGuid(),
                        Cinsi = itm.Cinsi,
                        StokKodu = itm.VarsayilanStokKodu,
                        StokAdi = itm.VarsayilanStokAdi,
                        Birim = itm.Birim,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        Miktar = itm.Miktar,
                        Aciklama = itm.Aciklama,
                        SipHId = hareket.Id,
                        RcAId = itm.RcAId,
                        RcDId = itm.Id
                    };
                    Detaylar.Add(det);
                }
                _mdl.Hareketler.Add(hareket);
                _mdl.Detaylar.InsertRange(0, Detaylar);
                myGrid1.DataSource = null;
                myGrid1.DataSource = _mdl.Hareketler;
                myView1.MoveLast();
                RecetePanelBagla();
            }
        }
        public void ReceteDegistir() {
            var data = myView1.MyGetCurrentItem<SiparisHareket>();
            if (data == null) {
                return;
            }


            var f = new FrmReceteSec {
                SecimIcinAcildi = true,
                WindowState = FormWindowState.Maximized
            };
            f.ShowDialog();
            if (f.Secildi) {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                var rec = _mngRecete.GetReceteKayit(st.Id);
                if (!rec.Success) {
                    MesajHata(rec.Message);
                    return;
                }

                var hareket = new SiparisHareket();
                var Detaylar = new List<SiparisHareketDetay>();
                var Recete = rec.Data;
                hareket.ReceteGrupKodu = "";
                hareket.ReceteKodu = Recete.Recete.ReceteKodu;
                hareket.ReceteAdi = Recete.Recete.ReceteAdi;
                hareket.RcAId = Recete.Recete.Id;
                hareket.Miktar = data.Miktar;
                hareket.StokKodu = Recete.Recete.EntegreStokKodu;
                hareket.StokAdi = Recete.Recete.EntegreStokAdi;
                hareket.Birim = Recete.Recete.EntegreBirim;
                var ilk = true;
                SiparisHareketDetay det;
                foreach (var itm in Recete.ReceteDetaylar) {
                    if (ilk) {
                        ilk = false;
                        hareket.Renk = itm.Renk;
                        hareket.Beden = itm.Beden;
                    }

                    if (!itm.SiparisdeGosterme) // siparişde göster varsa yeni kayıt olsun
                    {
                        hareket.YeniKayit = true;
                    }

                    det = new SiparisHareketDetay {
                        Id = MyGuid.NewGuid(),
                        Cinsi = itm.Cinsi,
                        StokKodu = itm.VarsayilanStokKodu,
                        StokAdi = itm.VarsayilanStokAdi,
                        Birim = itm.Birim,
                        Renk = itm.Renk,
                        Beden = itm.Beden,
                        Miktar = itm.Miktar,
                        Aciklama = itm.Aciklama,
                        SipHId = hareket.Id,
                        RcAId = itm.RcAId,
                        RcDId = itm.Id
                    };
                    Detaylar.Add(det);
                }

                /*   */

                _mdl.Hareketler.Remove(data);
                var sto = new List<SiparisHareketDetay>();
                var stl = _mdl.Detaylar.Where(c => c.SipHId != data.Id);
                foreach (var itm in stl) {
                    sto.Add(itm.Clone());
                }
                _mdl.Detaylar.Clear();
                _mdl.Detaylar.InsertRange(0, sto);



                /*   */


                _mdl.Hareketler.Add(hareket);
                _mdl.Detaylar.InsertRange(0, Detaylar);
                myGrid1.DataSource = null;
                myGrid1.DataSource = _mdl.Hareketler;
                myView1.MoveLast();
                RecetePanelBagla();
            }
        }
        private void ReceteGrupSec() {
            int adet = myView1.RowCount;
            FrmReceteGrupListesi f = new FrmReceteGrupListesi {
                SecimIcinAcildi = true,
                WindowState = FormWindowState.Maximized
            };
            f.ShowDialog();
            if (f.Secildi) {
                var recetGrup = ((ReceteGrup)f.SecilenRow).Clone();
                var receteGrDet = _mngReceteGrup.GetReceteKayit(recetGrup.Id);
                if (!receteGrDet.Success) {
                    MesajHata(receteGrDet.Message);
                    return;
                }
                var detayGrup = receteGrDet.Data;
                foreach (var detGrup in detayGrup.Detaylar) {
                    var rec = _mngRecete.GetReceteKayit(detGrup.RcAId);
                    if (!rec.Success) {
                        MesajHata(rec.Message);
                        return;
                    }

                    for (int i = 1; i <= detGrup.Miktar; i++) {

                        var hareket = new SiparisHareket();
                        var Detaylar = new List<SiparisHareketDetay>();
                        var Recete = rec.Data;
                        hareket.ReceteGrupKodu = recetGrup.ReceteGrupKodu;
                        hareket.ReceteKodu = Recete.Recete.ReceteKodu;
                        hareket.ReceteAdi = Recete.Recete.ReceteAdi;
                        hareket.RcAId = Recete.Recete.Id;
                        hareket.Miktar = 1;//detGrup.Miktar;
                        hareket.Aciklama = detGrup.Aciklama;
                        hareket.StokKodu = Recete.Recete.EntegreStokKodu;
                        hareket.StokAdi = Recete.Recete.EntegreStokAdi;
                        hareket.Birim = Recete.Recete.EntegreBirim;
                        SiparisHareketDetay det;
                        bool ilk = true;
                        foreach (ReceteDetay itm in Recete.ReceteDetaylar) {
                            if (ilk) {
                                ilk = false;
                                hareket.Renk = itm.Renk;
                                hareket.Beden = itm.Beden;
                            }

                            if (!itm.SiparisdeGosterme) // siparişde göster varsa yeni kayıt olsun
                            {
                                hareket.YeniKayit = true;
                            }

                            det = new SiparisHareketDetay {
                                Id = MyGuid.NewGuid(),
                                Cinsi = itm.Cinsi,
                                StokKodu = itm.VarsayilanStokKodu,
                                StokAdi = itm.VarsayilanStokAdi,
                                Birim = itm.Birim,
                                Renk = itm.Renk,
                                Beden = itm.Beden,
                                Miktar = itm.Miktar,
                                Aciklama = itm.Aciklama,
                                SipHId = hareket.Id,
                                RcAId = itm.RcAId,
                                RcDId = itm.Id
                            };
                            Detaylar.Add(det);
                        }

                        _mdl.Hareketler.Add(hareket);
                        _mdl.Detaylar.InsertRange(0, Detaylar);
                    }
                }
                myGrid1.DataSource = null;
                myGrid1.DataSource = _mdl.Hareketler;
                if (adet <= 0) {
                    myGrid1.GridYerlesimYukle();
                    if (Ortak.PlKapat) {
                        myView1.SutunGizle("Parti");
                        myView1.SutunGizle("Lot");
                    }
                }
            }
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (!degistirilemez) {
                RecetePanelBagla();
            }

        }

        private void TxtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            FrmMikroCariListesi f = new FrmMikroCariListesi();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi) {
                var rw = (MikroCari)f.SecilenRow;
                TxtCariKodu.Text = rw.CariKodu;
                TxtCariUnvani.Text = rw.CariUnvani1;
            }
        }

        private void BtnCariTemizle_Click(object sender, EventArgs e) {
            TxtCariKodu.Text = "";
            TxtCariUnvani.Text = "";
        }
    }
}