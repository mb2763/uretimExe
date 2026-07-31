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
                SolMenuOlustur();
                MdiTemaKancasiKur();
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
        // ---- Acilan tum MDI formlarini tek yerden guzellestir (Tema.cs) ----
        private readonly System.Collections.Generic.HashSet<Form> _temaliFormlar = new System.Collections.Generic.HashSet<Form>();
        private bool _temaKancasiKuruldu = false;
        private void MdiTemaKancasiKur() {
            if (_temaKancasiKuruldu) return;
            _temaKancasiKuruldu = true;
            this.MdiChildActivate -= FrmAna_MdiChildActivate;
            this.MdiChildActivate += FrmAna_MdiChildActivate;
        }
        private void FrmAna_MdiChildActivate(object sender, EventArgs e) {
            Form c = this.ActiveMdiChild;
            if (c == null) return;
            if (c.Name == "Masaustu") return;                       // masaustu/favoriler formuna dokunma
            if (c.GetType().Name == "FrmUretimEmriListesi") return; // kendi ozel temasi var
            if (_temaliFormlar.Contains(c)) return;
            _temaliFormlar.Add(c);
            try { Tema.FormGuzellestir(c); } catch { }
            c.FormClosed += delegate { try { _temaliFormlar.Remove(c); } catch { } };
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
            // Guvenli orijinal tema - bozuk/uyumsuz INI skin degerini gecersiz kilar
            try { DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful"); } catch { }
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
        // ================= SOL MENU (ozel: mavi + daireli zemin) - islem sirasina gore =================
        private DevExpress.XtraBars.Navigation.AccordionControl accSolMenu; // eski - kullanilmiyor
        private System.Windows.Forms.Panel pnlMenu;
        private readonly System.Collections.Generic.List<MnGrup> _mnGruplar = new System.Collections.Generic.List<MnGrup>();
        private class MnOge { public System.Windows.Forms.Label Lbl; public System.Windows.Forms.PictureBox Ikon; }
        private class MnGrup
        {
            public System.Windows.Forms.Label Baslik;
            public System.Windows.Forms.PictureBox BaslikIkon;
            public System.Windows.Forms.Label Cevron;
            public System.Collections.Generic.List<MnOge> Ogeler = new System.Collections.Generic.List<MnOge>();
            public bool Acik;
        }

        /// <summary>Verilen hint'e (FormSec anahtari) karsilik gelen ribbon butonunu bulur (ikon/ad icin).</summary>
        public DevExpress.XtraBars.BarButtonItem MenuButonBul(string hint)
        {
            if (string.IsNullOrEmpty(hint)) return null;
            foreach (DevExpress.XtraBars.BarItem it in Rib_Ana1.Items)
            {
                var bi = it as DevExpress.XtraBars.BarButtonItem;
                if (bi != null && bi.Hint != null && bi.Hint.ToString().Trim().Equals(hint.Trim(), System.StringComparison.OrdinalIgnoreCase))
                    return bi;
            }
            return null;
        }

        private void SolMenuOlustur() {
            if (pnlMenu != null) return;
            pnlMenu = new System.Windows.Forms.Panel();
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Dock = DockStyle.Fill;
            pnlMenu.AutoScroll = true;
            pnlMenu.BackColor = System.Drawing.Color.FromArgb(18, 36, 74);
            var menuBg = System.IO.Path.Combine(Application.StartupPath, "Resimler", "senkron-menu-bg.png");
            if (System.IO.File.Exists(menuBg))
            {
                pnlMenu.BackgroundImage = System.Drawing.Image.FromFile(menuBg);
                pnlMenu.BackgroundImageLayout = ImageLayout.Stretch;
            }
            pnlMenu.Resize += (s, e) => MenuYerlestir();

            SolGrup("1.  ÜRETİM", true, new object[] {
                "İş Emirleri", BarBtnUretimEmirleri,
                "Operasyon Takip", BarBtnUretimTakip,
                "İstasyon Takip", BarBtnIstasyonTakip,
                "Üretim Girişi", BarBtnUretimEkle,
                "Üretim Talepleri", BarBtnUretimTalepler,
            });
            SolGrup("2.  MAL KABUL & KONTROL", false, new object[] {
                "Mal Kabul", BarBtnMalKabulListe,
                "Ölçü Kontrol", BarBtnOlcuKontrol,
            });
            SolGrup("3.  İSTASYON İŞLEMLERİ", false, new object[] {
                "İstasyon Hareketleri", BarBtnIstasyonHareketler,
                "İstasyon Bekleyenler", BarBtnIstasyonBekleyenler,
                "İstasyon Fişler", BarBtnIstasyonFisler,
                "Hızlı Üretim", BarBtnHizliUretim,
            });
            SolGrup("4.  MİKRO", false, new object[] {
                "Siparişler", BarBtnMikroSiparisler,
                "Cariler", BarBtnMikroCariler,
                "Stoklar", BarBtnMikroStoklar,
            });
            SolGrup("5.  REÇETE & KARTLAR", false, new object[] {
                "Reçete Listesi", BarBtnReceteListesi,
                "Reçete Ekle", BarBtnReceteEkle,
                "Operasyon Kartları", BarBtnOperasyonKartlari,
                "İstasyon Kartları", BarBtnIstasyonKartlari,
                "İstasyon-Grup Kodları", BarBtnReceteIstasyonGruplar,
            });
            SolGrup("6.  RAPORLAR", false, new object[] {
                "İstasyon Raporu", BarBtnIstasyonRaporu,
                "Stok Tüketim Raporu", BarBtnStokTuketimRaporu,
                "Reçete Genel Rapor", BarBtnReceteGenelRaporu,
            });
            SolGrup("7.  TANIMLAR & DİĞER", false, new object[] {
                "Personel Kartları", BarBtnPersonelListesi,
                "İstasyon Bakımları", BarBtnIstasyonBakimList,
                "Durdurma Kodları", BarBtnIstasyonDurdurmaKodlari,
                "Tablet Acil Mesaj", BarBtnAcilMesaj,
            });
            SolGrup("8.  AYARLAR", false, new object[] {
                "Kullanıcılar", BarBtnKullanicilar,
                "Genel Ayarlar", BarBtnGenelAyarlar,
                "Mikro Entegrasyon", BarBtnMikroEntAyarlari,
                "Mail Ayarları", BarBtnMailAyarlari,
                "Veritabanı Güncelle", BarBtnDbGuncelle,
            });

            MenuYerlestir();

            // ----- Sol panel: ust marka serisi + altinda menu -----
            var pnlSol = new Panel();
            pnlSol.Name = "pnlSolMenu";
            pnlSol.Dock = DockStyle.Left;
            pnlSol.Width = 280;

            var pnlBaslik = new Panel();
            pnlBaslik.Dock = DockStyle.Top;
            pnlBaslik.Height = 78;
            pnlBaslik.BackColor = System.Drawing.Color.FromArgb(12, 26, 60);
            var hdrPath = System.IO.Path.Combine(Application.StartupPath, "Resimler", "senkron-header.png");
            if (System.IO.File.Exists(hdrPath))
            {
                pnlBaslik.BackgroundImage = System.Drawing.Image.FromFile(hdrPath);
                pnlBaslik.BackgroundImageLayout = ImageLayout.Stretch;
            }

            pnlSol.Controls.Add(pnlMenu);
            pnlSol.Controls.Add(pnlBaslik);
            this.Controls.Add(pnlSol);
            pnlSol.BringToFront();

            // ----- Ana alan (MDI) arka plani: desenli + filigran -----
            try
            {
                var bgPath = System.IO.Path.Combine(Application.StartupPath, "Resimler", "senkron-bg.png");
                foreach (Control c in this.Controls)
                {
                    var mc = c as MdiClient;
                    if (mc != null)
                    {
                        if (System.IO.File.Exists(bgPath))
                        {
                            mc.BackgroundImage = System.Drawing.Image.FromFile(bgPath);
                            mc.BackgroundImageLayout = ImageLayout.Stretch;
                        }
                        else
                        {
                            mc.BackColor = System.Drawing.Color.FromArgb(236, 240, 248);
                        }
                        break;
                    }
                }
            }
            catch { }

            Rib_Ana1.Visible = false; // ust ribbon menusunu gizle - sol menu yeterli

            PencereKontrolleriEkle();
        }

        /// <summary>Icerik alaninin ustune baslik + pencere kontrol (kucult/kapat) barini ekler.</summary>
        private void PencereKontrolleriEkle()
        {
            var topBar = new Panel();
            topBar.Name = "topBar";
            topBar.Height = 42;
            topBar.Dock = DockStyle.Top;
            topBar.BackColor = System.Drawing.Color.FromArgb(15, 32, 64);

            var lblTitle = new Label();
            lblTitle.AutoSize = false;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            lblTitle.Padding = new Padding(20, 0, 0, 0);
            lblTitle.ForeColor = System.Drawing.Color.FromArgb(188, 206, 234);
            lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular);
            lblTitle.Text = "SENKRON ERP  ·  Üretim Yönetim Sistemi";

            var btnKapat = new Button();
            btnKapat.Text = "✕";
            btnKapat.Size = new System.Drawing.Size(50, 30);
            btnKapat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKapat.FlatStyle = FlatStyle.Flat;
            btnKapat.FlatAppearance.BorderSize = 0;
            btnKapat.BackColor = System.Drawing.Color.FromArgb(197, 57, 41);
            btnKapat.ForeColor = System.Drawing.Color.White;
            btnKapat.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            btnKapat.Cursor = Cursors.Hand;
            btnKapat.TabStop = false;
            btnKapat.Click += (s, e) => { this.Close(); };

            var btnKucult = new Button();
            btnKucult.Text = "—";
            btnKucult.Size = new System.Drawing.Size(50, 30);
            btnKucult.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnKucult.FlatStyle = FlatStyle.Flat;
            btnKucult.FlatAppearance.BorderSize = 0;
            btnKucult.BackColor = System.Drawing.Color.FromArgb(40, 56, 86);
            btnKucult.ForeColor = System.Drawing.Color.White;
            btnKucult.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            btnKucult.Cursor = Cursors.Hand;
            btnKucult.TabStop = false;
            btnKucult.Click += (s, e) => { this.WindowState = FormWindowState.Minimized; };

            lblTitle.BackColor = System.Drawing.Color.FromArgb(15, 32, 64);
            topBar.Controls.Add(lblTitle);
            this.Controls.Add(topBar);
            topBar.BringToFront();

            // Pencere butonlari: panel YOK - dogrudan formun uzerine, sabit sag-ust, en onde (kesin gorunur)
            btnKapat.SetBounds(System.Math.Max(10, this.ClientSize.Width - 52), 6, 46, 28);
            btnKucult.SetBounds(System.Math.Max(10, this.ClientSize.Width - 102), 6, 46, 28);
            this.Controls.Add(btnKapat);
            this.Controls.Add(btnKucult);
            btnKapat.BringToFront();
            btnKucult.BringToFront();
        }

        private void SolGrup(string baslik, bool acik, object[] ogeler) {
            var g = new MnGrup();
            g.Acik = acik;

            var h = new System.Windows.Forms.Label();
            h.AutoSize = false;
            h.BackColor = System.Drawing.Color.Transparent;
            h.ForeColor = System.Drawing.Color.White;
            h.Font = new System.Drawing.Font("Bahnschrift SemiBold", 11.5F, System.Drawing.FontStyle.Bold);
            h.Text = baslik;
            h.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            h.Padding = new System.Windows.Forms.Padding(44, 0, 0, 0);
            h.Cursor = System.Windows.Forms.Cursors.Hand;

            var hIkon = new System.Windows.Forms.PictureBox();
            hIkon.BackColor = System.Drawing.Color.Transparent;
            hIkon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            hIkon.Cursor = System.Windows.Forms.Cursors.Hand;

            var cev = new System.Windows.Forms.Label();
            cev.AutoSize = false;
            cev.BackColor = System.Drawing.Color.Transparent;
            cev.ForeColor = System.Drawing.Color.FromArgb(150, 180, 220);
            cev.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            cev.Text = acik ? "▲" : "▼";
            cev.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            cev.Cursor = System.Windows.Forms.Cursors.Hand;

            g.Baslik = h; g.BaslikIkon = hIkon; g.Cevron = cev;

            System.Drawing.Image grupIkon = null;
            for (int i = 0; i + 1 < ogeler.Length; i += 2) {
                var metin = ogeler[i] as string;
                var btn = ogeler[i + 1] as DevExpress.XtraBars.BarItem;
                var svg = btn != null ? btn.ImageOptions.SvgImage : null;

                var lbl = new System.Windows.Forms.Label();
                lbl.AutoSize = false;
                lbl.BackColor = System.Drawing.Color.Transparent;
                lbl.ForeColor = System.Drawing.Color.FromArgb(206, 220, 242);
                lbl.Font = new System.Drawing.Font("Bahnschrift SemiBold", 11F, System.Drawing.FontStyle.Regular);
                lbl.Text = metin;
                lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                lbl.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
                lbl.Cursor = System.Windows.Forms.Cursors.Hand;

                var ikon = new System.Windows.Forms.PictureBox();
                ikon.BackColor = System.Drawing.Color.Transparent;
                ikon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                ikon.Cursor = System.Windows.Forms.Cursors.Hand;
                var im = SvgToImage(svg, 24);
                if (im != null) { ikon.Image = im; if (grupIkon == null) grupIkon = SvgToImage(svg, 26); }

                var hedef = btn;
                System.EventHandler ac = (s, ev) => { if (hedef != null) hedef.PerformClick(); };
                lbl.Click += ac; ikon.Click += ac;
                var refLbl = lbl;
                System.EventHandler gir = (s, ev) => { refLbl.ForeColor = System.Drawing.Color.White; };
                System.EventHandler cik = (s, ev) => { refLbl.ForeColor = System.Drawing.Color.FromArgb(206, 220, 242); };
                lbl.MouseEnter += gir; lbl.MouseLeave += cik; ikon.MouseEnter += gir; ikon.MouseLeave += cik;

                pnlMenu.Controls.Add(lbl);
                pnlMenu.Controls.Add(ikon);
                g.Ogeler.Add(new MnOge { Lbl = lbl, Ikon = ikon });
            }
            if (grupIkon != null) hIkon.Image = grupIkon;

            System.EventHandler tog = (s, ev) => { g.Acik = !g.Acik; cev.Text = g.Acik ? "▲" : "▼"; MenuYerlestir(); };
            h.Click += tog; cev.Click += tog; hIkon.Click += tog;

            pnlMenu.Controls.Add(h);
            pnlMenu.Controls.Add(hIkon);
            pnlMenu.Controls.Add(cev);
            _mnGruplar.Add(g);
        }

        private void MenuYerlestir() {
            if (pnlMenu == null) return;
            int w = pnlMenu.ClientSize.Width; if (w < 80) w = 280;
            int y = 8;
            pnlMenu.SuspendLayout();
            foreach (var g in _mnGruplar) {
                g.Baslik.SetBounds(0, y, w, 46);
                g.BaslikIkon.SetBounds(14, y + 11, 24, 24);
                g.Cevron.SetBounds(w - 34, y, 26, 46);
                g.Baslik.BringToFront(); g.BaslikIkon.BringToFront(); g.Cevron.BringToFront();
                y += 46;
                foreach (var o in g.Ogeler) {
                    o.Lbl.Visible = g.Acik; o.Ikon.Visible = g.Acik;
                    if (g.Acik) {
                        o.Lbl.SetBounds(0, y, w, 38);
                        o.Ikon.SetBounds(22, y + 8, 22, 22);
                        o.Lbl.BringToFront(); o.Ikon.BringToFront();
                        y += 38;
                    }
                }
                y += 6;
            }
            pnlMenu.ResumeLayout();
        }

        private System.Drawing.Image SvgToImage(DevExpress.Utils.Svg.SvgImage svg, int size) {
            if (svg == null) return null;
            try { return new DevExpress.Utils.Svg.SvgBitmap(svg).Render(new System.Drawing.Size(size, size), (DevExpress.Utils.Design.ISvgPaletteProvider)null); }
            catch { return null; }
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