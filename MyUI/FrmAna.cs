using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars.Localization;
using My.Business;
using My.Business.Service.Depolar;
using My.Business.Service.Templer;
using My.Core;
using My.Entities.IstasyonAciklamalar;
using My.Entities.Templer;
using My.Kontrol.Formlar;
using MyUI.Aciklamalar;
using MyUI.AyarVeGenel;
using MyUI.AyarVeGenelModul;
using MyUI.HizliUretimModule;
using MyUI.IstasyonModul;
using MyUI.KullaniciModule;
using MyUI.MailModule;
using MyUI.MalKabul;
using MyUI.PersonelModule;
using MyUI.Raporlar.ReceteRaporlari;
using MyUI.Raporlar.UretimRaporlari;
using MyUI.ReceteIstasyonGrupModul;
using MyUI.SmsModule;
using MyUI.Updates;
using MyUI.UretimKontroller;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MyUI {
    public partial class FrmAna : MyFrmAna {
        private FrmMasaustu1 frmmasaUstu;
        public string conStr { get; set; }
        ITempMikroStokService _TmpMikroStok;
        public FrmAna() {
            InitializeComponent();
            Rib_Ana1.Manager.HighlightedLinkChanged += RibbonControl1_HighlightedLinkChanged;
            Application.DoEvents();
            Application.DoEvents();
            Rib_Ana1.Minimized = true;
            this.Load += FrmAna_Load;
        }
        /// <summary>
        /// true ise çık hata vermesin
        /// </summary>
        /// <returns></returns>
        bool UpdateKontrol() {
            var knt = new UpdateProgram();
            var k = knt.VersiyonKontrol();
            return k;
        }
        private void FrmAna_Load(object sender, EventArgs e) {
            Alt_Bar_Yazilim.Caption = "Senkron";
            Application.DoEvents();
            if (UpdateKontrol()) {
                return;
            }
            Application.DoEvents();
            FrmLogin fr = new FrmLogin();
            fr.ShowDialog();
            if (fr.GirisYapildi) {
                Cursor.Current = Cursors.WaitCursor;
                SkinAdd();
                MasaUstuAc();
                TmrOpacity.Start();
                Alt_Bar_UserName.Caption = Ortak.KullaniciAdi;
                if (OrtakLis.SistemLisansKontrolu()) {
                    Ortak.LisansAktif = true;
                    Alt_Bar_SubeKodu.Caption = "Lisans = Aktif";
                    BarBtnReceteListesi.Enabled = true;
                    BarBtnReceteGrupTakimlar.Enabled = true;
                    BarBtnKullanicilar.Enabled = true;
                }
                else {
                    Ortak.LisansAktif = false;
                    Alt_Bar_SubeKodu.Caption = "Lisans = Pasif";
                    BarBtnReceteListesi.Enabled = false;
                    BarBtnReceteGrupTakimlar.Enabled = false;
                    BarBtnKullanicilar.Enabled = false;
                }
                Ortak.DbPro = new DatabaseFactoryPro(Ortak.GetKey(), ProgramYolAyarlari.DbProAdi);
                Ortak.DbMikro = new DatabaseFactoryMikro(Ortak.GetKey(), ProgramYolAyarlari.DbMikroAdi);
                this.WindowState = FormWindowState.Maximized;
                Ortak.MasaUstuKapanmasin = true;
                Cursor.Current = Cursors.Default;
                Alt_Bar_FirmaDb.Caption = Ortak.DbPro.Settings.Database;
                Ortak.MikroEntAyarlarBagla();
                Ortak.GenelAyarlarBagla();
                Ortak.IstasyonAyarlarBagla();
                barStaticItem1.Caption = GetVersiyon();
                _TmpMikroStok = Ortak.DbPro.TempMikroStok;
                TempGuncelle();
                return;
            }
            Cursor.Current = Cursors.Default;
            this.Close();
        }
        private void TempGuncelle() {
            Cursor.Current = Cursors.WaitCursor;

            var rs = _TmpMikroStok.MikroStokKategoriGuncelle(Ortak.DbMikro.Settings.Database);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            var rs1 = _TmpMikroStok.MikroStokGuncelle(Ortak.DbMikro.Settings.Database);
            if (!rs1.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs1.Message);
                return;
            }
           
            Cursor.Current = Cursors.Default;
        }

        string GetVersiyon() {
            string versiyon = "0";
            if (File.Exists("versiyon.txt")) {
                versiyon = File.ReadAllText("versiyon.txt");
            } 
            return "V:" +versiyon;
        }
        private void TmrOpacity_Tick(object sender, EventArgs e) {
            if (this.Opacity < 1.0) this.Opacity += 0.1;
            else {
                TmrOpacity.Enabled = false; // % 100 olduğunda timer duruyor.
                TmrOpacity.Stop();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void SkinAdd() {
            try {
                var ApplicationSkinName = Ortak.AyarIni.Oku("Theme", "ApplicationSkinName", "");
                var ApplicationSkinPaletteName = Ortak.AyarIni.Oku("Theme", "ApplicationSkinPaletteName", "");
                SkinHelper.InitSkinPaletteGallery(skinPaletteRibbonGalleryBarItem1);
                SkinHelper.InitSkinGallery(skinRibbonGalleryBarItem1);
                UserLookAndFeel.Default.SkinName = ApplicationSkinName.ToString();
                var skin = CommonSkins.GetSkin(UserLookAndFeel.Default);
                DevExpress.Utils.Svg.SvgPalette palette = skin.CustomSvgPalettes[ApplicationSkinPaletteName];
                skin.SvgPalettes[Skin.DefaultSkinPaletteName].SetCustomPalette(palette);
                LookAndFeelHelper.ForceDefaultLookAndFeelChanged();
            } catch {
                Ortak.AyarIni.Yaz("Theme", "ApplicationSkinName", UserLookAndFeel.Default.SkinName);
                Ortak.AyarIni.Yaz("Theme", "ApplicationSkinPaletteName", skinPaletteRibbonGalleryBarItem1.Gallery.GetCheckedItem().Caption);
            }
        }
        private void FrmAnaV2_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                Ortak.AyarIni.Yaz("Theme", "ApplicationSkinName", UserLookAndFeel.Default.SkinName);
                Ortak.AyarIni.Yaz("Theme", "ApplicationSkinPaletteName", skinPaletteRibbonGalleryBarItem1.Gallery.GetCheckedItem().Caption);
                //Ortak.MasaUstuKapanmasin = false;
            } catch { }
            if (!MesajSor("Programı Kapatmak İstiyormusunuz..")) {
                e.Cancel = true;
            }
        }
        private string AktifButon = "";
        public void AktifButonSec(object sender) {
            if (sender.GetType() != typeof(BarButtonItem)) {
                // AktifButon = "";
                return;
            }
            var btn = sender as BarButtonItem;
            if (string.IsNullOrEmpty(btn.Name)) return;
            if (btn.Hint == null) return;
            AktifButon = btn.Hint.ToString();
            this.Text = AktifButon;
        }
        public void MasaUstuAc() {
            if (Application.OpenForms["Masaustu"] == null) {
                frmmasaUstu = new FrmMasaustu1();
                frmmasaUstu.Name = "Masaustu";
                frmmasaUstu.MdiParent = this;
                frmmasaUstu.Show();
            }
            else {
                var frm = Application.OpenForms["Masaustu"];
                frm.Hide();
                frm.Show();
            }
        }
        /*   Bar Butonlar    */
        private void BarBtnlar_Click(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            
            if (string.IsNullOrEmpty(e.Item.Hint)) return;
            AktifButon = e.Item.Hint.ToString();
            var tooltip = e.Item.Hint.ToString();
            if (string.IsNullOrEmpty(tooltip.Trim())) return;
            FormAc.FormSec(tooltip);
        }
        void RibbonControl1_HighlightedLinkChanged(object sender, DevExpress.XtraBars.HighlightedLinkChangedEventArgs e) {
            if (e.Link == null) return;
            if (e.Link.Item.Hint != null) {
                AktifButonSec(e.Link.Item);
            }
        }
        private void RibbonControl1_ShowCustomizationMenu(object sender, DevExpress.XtraBars.Ribbon.RibbonCustomizationMenuEventArgs e) {
           
            if (e.Link == null) {
                BarItemLink menuAboutCommand1 = e.CustomizationMenu.ItemLinks.Where(link => link.Caption == "Tüm Sekmeleri Kapat").FirstOrDefault();
                if (menuAboutCommand1 == null) {
                    menuAboutCommand1 = e.CustomizationMenu.AddItem(GetTumSekmeleriKapatCommand());
                    menuAboutCommand1.BeginGroup = true;
                }
                return;
            }
            else {
                BarItemLink linkAddToQat = e.CustomizationMenu.ItemLinks.Where(link => link.Caption == BarLocalizer.Active.GetLocalizedString(BarString.RibbonToolbarAdd)).FirstOrDefault();
                linkAddToQat.Visible = false;
                BarItemLink menuAboutCommand = e.CustomizationMenu.ItemLinks.Where(link => link.Caption == "Hızlı Başlangıca Ekle").FirstOrDefault();
                if (menuAboutCommand == null) {
                    menuAboutCommand = e.CustomizationMenu.AddItem(GetHizliBaslangicCommand());
                    menuAboutCommand.BeginGroup = true;
                }
                BarItemLink menuAboutCommand1 = e.CustomizationMenu.ItemLinks.Where(link => link.Caption == "Tüm Sekmeleri Kapat").FirstOrDefault();
                if (menuAboutCommand1 == null) {
                    menuAboutCommand1 = e.CustomizationMenu.AddItem(GetTumSekmeleriKapatCommand());
                    menuAboutCommand1.BeginGroup = true;
                }
            }


        }
        BarItem hizliBaslangicItem;
        BarItem tumSekmeleriKapatItem;
        private BarItem GetHizliBaslangicCommand() {
            if (hizliBaslangicItem == null) {
                hizliBaslangicItem = new BarButtonItem();
                hizliBaslangicItem.Caption = "Hızlı Başlangıca Ekle";
                hizliBaslangicItem.ItemClick += new ItemClickEventHandler(HizliBaslangicaEkle_ItemClick);
                Rib_Ana1.Items.Add(hizliBaslangicItem);
            }
            return hizliBaslangicItem;
        }
        private BarItem GetTumSekmeleriKapatCommand() {
            if (tumSekmeleriKapatItem == null) {
                tumSekmeleriKapatItem = new BarButtonItem();
                tumSekmeleriKapatItem.Caption = "Tüm Sekmeleri Kapat";
                tumSekmeleriKapatItem.ItemClick += new ItemClickEventHandler(TumSekmeleriKapat_ItemClick);
                Rib_Ana1.Items.Add(tumSekmeleriKapatItem);
            }
            return tumSekmeleriKapatItem;
        }
        // The method invoked when the "About" command is clicked.
        void HizliBaslangicaEkle_ItemClick(object sender, ItemClickEventArgs e) {
            if (string.IsNullOrEmpty(AktifButon.ToString().Trim())) {
                return;
            }
            frmmasaUstu.ButonEkle(AktifButon);
            AktifButon = "";
        }
        void TumSekmeleriKapat_ItemClick(object sender, ItemClickEventArgs e) {
            if (string.IsNullOrEmpty(AktifButon.ToString().Trim())) {
                return;
            }
            if (MesajSor("Tüm Sekmeleri Kapatmak istiyormusunuz..")) {

                foreach (Form frm in this.MdiChildren) {
                    if (frm != Parent) {
                        if (frm.Name != "Masaustu") {
                            frm.Close();
                        }

                    }

                }
            }

        }
        private void BarBtnKullanicilar_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var f = new FrmKullaniciKayit();
            f.ShowDialog();
        }
        private void BarBtnPersonelListesi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            var f = new FrmPersonelKartlari();
            f.ShowDialog();
        }

        private void BtnLisansKayit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            Ortak.LisansKayit();
        }

        private void BarBtnMailAyarlari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmMailSettings();
            f.ShowDialog();
        }

        private void BarBtnIstasyonBaslatmaHatalari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonAciklamalari();
            f.AciklamaModulTuru = IstasyonAciklamaModulTuru.IstasyonBaslatmaHata;
            f.ShowDialog();
        }

        private void BarBtnIstasyonDurdurmaKodlari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonAciklamalari();
            f.AciklamaModulTuru = IstasyonAciklamaModulTuru.IstasyonDurdurmaKodu;
            f.ShowDialog();
        }

        private void BarBtnIstasyonFireSebepleri_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonAciklamalari();
            f.AciklamaModulTuru = IstasyonAciklamaModulTuru.IstasyonFireSebep;
            f.ShowDialog();
        }

        private void BarBtnMikroEntAyarlari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmMikroEntAyarlari();
            f.ShowDialog();
        }
        private void BarBtnIstasyonUretimAyarlari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonUretimAyarlari();
            f.ShowDialog();
        }
        private void BarBtnDbGuncelle_ItemClick(object sender, ItemClickEventArgs e) {
            Ortak.DatabaseGuncelleUretim();
        }

        private void BarBtnReceteAciklama_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmAciklamaKodlar();
            f.AciklamaModulTuru = My.Entities.UretimAciklamalar.AciklamaModulTuru.ReceteAciklama;
            f.ShowDialog();
        }

        private void BarBtnIstasyonHareketLog_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonHareketLogList();
            f.MdiParent = this;
            f.Show();
        } 
        private void BarBtnAcilMesaj_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmMesajGenel();

            f.ShowDialog();
        }
        private void BarBtnIstasyonBakimList_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmIstasyonBakimList();
            f.MdiParent = this;
            f.Show();
        }
        private void BarUstTumSekmeleriKapat_ItemClick(object sender, ItemClickEventArgs e) {
            if (MesajSor("Tüm Sekmeleri Kapatmak istiyormusunuz..")) { 
                foreach (Form frm in this.MdiChildren) {
                    if (frm != Parent) {
                        if (frm.Name != "Masaustu") {
                            frm.Close();
                        }
                    }

                }
            }
        }

        private void BarBtnSmsRapor_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmSmsRapor();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnSmsAyarlari_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmSmsAyarlari();
            f.ShowDialog();
        }

        private void BarBtnReceteIstasyonGruplar_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmReceteIstasyonGrupKodlari();
            f.ShowDialog();
        }

        private void BarBtnReceteIstasyonGrupOperasyonlar_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmReceteIstasyonGrupOperasyonEslestir();
            f.ShowDialog();
        }

        private void BarBtnMalKabulListe_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmMalKabulListe();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnOlcuKontrol_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmOlcuKontrolListesi();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnHizliUretim_ItemClick(object sender, ItemClickEventArgs e) {

            var f = new FrmHizliUretimEG();
            //f.MdiParent = this;
            f.ShowDialog();
        }

        private void AltbarTestButton1_ItemClick(object sender, ItemClickEventArgs e) {
            var rs = Ortak.DbPro.Depolar.GetCount();
            if (rs.IsError) {
                MesajHata(rs.Message);
                return;
            }
            AltbarTestButton1.Caption = rs.Data.ToString();
        }

        private void BarBtnStokTuketimRaporu_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmUretimStokTuketimRaporu();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnStokTuketimRaporuDetayli_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmUretimStokTuketimRaporuDetayli();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnReceteKullanilanStok_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmReceteKullanilanStokList();
            f.MdiParent = this;
            f.Show();
        }

        private void BarBtnGenelAyarlar_ItemClick(object sender, ItemClickEventArgs e) {
             var f = new FrmGenelAyarlar();            
            f.ShowDialog();
        }

        private void BarBtnReceteGenelRaporu_ItemClick(object sender, ItemClickEventArgs e) {
            var f = new FrmReceteRaporu();
            f.MdiParent = this;
            f.Show();
        }

        private void btnStokGuncelle_ItemClick(object sender, ItemClickEventArgs e) {
            FrmStokKodGuncelle f = new FrmStokKodGuncelle();
            f.ShowDialog();
            }
        }
}