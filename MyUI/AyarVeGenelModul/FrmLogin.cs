using My.Business;
using My.Core;
using My.Entities.Kullanicilar;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
namespace MyUI
{
    public partial class FrmLogin : MyFrmLoginPaneli
    {
        public bool GirisYapildi = false;
        public string KullaniciKodu = "";
        public Kullanici _rw;
        private string _aesMasterKey = "";
        private string ProDbAdi = My.Core.ProgramYolAyarlari.DbProAdi;
        private string MikroDbAdi = My.Core.ProgramYolAyarlari.DbMikroAdi;
        public FrmLogin()
        {
            InitializeComponent();
            this.Load += new EventHandler(this.MyFrmGiris_Load);
            BtnDbPro.Click += new EventHandler(this.BtnDbPro_Click);
            BtnDbMikro.Click += new EventHandler(this.BtnDbMikro_Click);
            BtnGiris.Click += new EventHandler(this.BtnGiris_Click);
            BtnKapat.Click += new EventHandler(this.BtnKapat_Click);
            this.TxtKullanici.KeyDown += new KeyEventHandler(this.TxtKullanici_KeyDown);
            this.TxtSifre.KeyDown += new KeyEventHandler(this.TxtSifre_KeyDown);
        }
        private void MyFrmGiris_Load(object sender, EventArgs e)
        {
            TasarimUygula();
            BtnDbPro.Visible = false;
            BtnDbMikro.Visible = false;
            LblVersiyon.Text = GetVersiyon();
            _aesMasterKey = Ortak.GetKey();
            HataKontrolu(() =>
           {
               Ortak.DbPro = new DatabaseFactoryPro(_aesMasterKey, ProDbAdi);
               Ortak.DbMikro = new DatabaseFactoryMikro(_aesMasterKey, MikroDbAdi);
               TxtKullanici.Text = Ortak.AyarIni.Oku("AYAR", "KULLANICI", "");
               TxtSifre.Focus();
           });
        }
        string GetVersiyon() {
            string versiyon = "0";
            if (File.Exists("versiyon.txt")) {
                versiyon = File.ReadAllText("versiyon.txt");
            }
            return "V:" + versiyon;
        }
        public static void HataKontrolu(Action action)
        {
            GetKontrol(() => { action(); });
        }
        private void BtnDbPro_Click(object sender, EventArgs e)
        {
            HataKontrolu(() =>
            {
                //FrmDataPaneli frm = new FrmDataPaneli(ProDbAdi);
                //frm.ShowDialog();
                //if (frm.AyarDegisti)
                //{
                //    Ortak.DbPro = new DatabaseFactoryPro(Ortak.ANA_KEY, ProDbAdi);
                //}
            });
        }
        private void BtnDbMikro_Click(object sender, EventArgs e)
        {
            HataKontrolu(() =>
           {
               //FrmDataPaneli frm = new FrmDataPaneli(MikroDbAdi);
               //frm.ShowDialog();
               //if (frm.AyarDegisti)
               //{
               //    Ortak.DbMikro = new DatabaseFactoryMikro(Ortak.ANA_KEY, MikroDbAdi);
               //}
           });
        }
        private void BtnGiris_Click(object sender, EventArgs e)
        {
            if (Kontrol() == true)
            {
                GirisYapildi = true;
                Ortak.AyarIni.Yaz("AYAR", "KULLANICI", TxtKullanici.Text);
                KullaniciKodu = TxtKullanici.Text.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı Veya Şifre Geçersiz", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void BtnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool Kontrol()
        {
            var ad = Ortak.DbPro.Kullanicilar.Query<int>("select count(*) as Adet from Kullanici ", null);
            if (ad.Success && ad.Data.Count() > 0)
            {
                if (ad.Data.ToList().FirstOrDefault() <= 0)
                {
                    Ortak.DbPro.Kullanicilar.Insert(new Kullanici() { KullaniciAdi = "Admin", Adi = "Admin", Id = MyGuid.NewGuid(), Sifre = "1".Sifrele(_aesMasterKey), Soyadi = "User", Admin = true });
                }
            }
            string ka = TxtKullanici.Text.ToString().ToUpper();
            string ps = TxtSifre.Text.ToString().ToUpper();
            string sif = ps.Sifrele(_aesMasterKey);
            var res = Ortak.DbPro.Kullanicilar.SelectFirst(k => k.KullaniciAdi == ka);
            if (res.Success)
            {
                _rw = res.Data;
                if (_rw == null)
                {
                    MessageBox.Show("Kullanici Bulunamadı");
                    return false;
                }
                if (ka == _rw.KullaniciAdi.ToUpper() && sif == _rw.Sifre)
                {
                    Ortak.KullaniciAdi = ka;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show(res.Message);
                return false;
            }
        }
        private void TxtSifre_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter))
            {
                if (Kontrol() == true)
                {
                    GirisYapildi = true;
                    Ortak.AyarIni.Yaz("AYAR", "KULLANICI", TxtKullanici.Text);
                    KullaniciKodu = TxtKullanici.Text.ToString();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Kullanıcı Adı Veya Şifre Geçersiz", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private void TxtKullanici_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtSifre.Focus();
                TxtSifre.SelectAll();
            }
        }

        private void LblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// SENKRONERP profesyonel login tasarimini uygular (arka plan gorseli + modern alanlar + giris butonu).
        /// </summary>
        private void TasarimUygula()
        {
            try
            {
                // Eski (CepPatron) arka plan resmini gizle
                var pics = this.Controls.Find("myPictureEdit1", true);
                if (pics.Length > 0) pics[0].Visible = false;

                // SENKRONERP arka plan gorseli
                var imgPath = Path.Combine(Application.StartupPath, "Resimler", "senkron-login.png");
                if (File.Exists(imgPath))
                {
                    this.BackgroundImage = Image.FromFile(imgPath);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else
                {
                    this.BackColor = Color.FromArgb(13, 27, 62);
                }
                this.ClientSize = new Size(460, 560);
                this.StartPosition = FormStartPosition.CenterScreen;

                // Kart ici karsilama basligi
                var lblHos = new Label
                {
                    Text = "Hoş Geldiniz",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(28, 40, 60),
                    Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                    Size = new Size(296, 24),
                    Location = new Point(82, 338)
                };
                this.Controls.Add(lblHos);

                // Alan etiketleri
                var lblK = new Label { Text = "Kullanıcı Adı", AutoSize = true, BackColor = Color.Transparent, ForeColor = Color.FromArgb(90, 100, 120), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(82, 366) };
                var lblS = new Label { Text = "Şifre", AutoSize = true, BackColor = Color.Transparent, ForeColor = Color.FromArgb(90, 100, 120), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), Location = new Point(82, 423) };
                this.Controls.Add(lblK);
                this.Controls.Add(lblS);

                // Kullanici alani
                TxtKullanici.BorderStyle = BorderStyle.FixedSingle;
                TxtKullanici.BackColor = Color.White;
                TxtKullanici.ForeColor = Color.FromArgb(33, 37, 41);
                TxtKullanici.Font = new Font("Segoe UI", 11.5F);
                TxtKullanici.TextAlign = HorizontalAlignment.Left;
                TxtKullanici.Location = new Point(82, 385);
                TxtKullanici.Size = new Size(296, 29);

                // Sifre alani
                TxtSifre.BorderStyle = BorderStyle.FixedSingle;
                TxtSifre.BackColor = Color.White;
                TxtSifre.ForeColor = Color.FromArgb(33, 37, 41);
                TxtSifre.Font = new Font("Segoe UI", 11.5F);
                TxtSifre.TextAlign = HorizontalAlignment.Left;
                TxtSifre.Location = new Point(82, 442);
                TxtSifre.Size = new Size(296, 29);
                TxtSifre.UseSystemPasswordChar = true;

                // Giris butonu (modern, hover'li)
                this.Controls.Add(BtnGiris);
                BtnGiris.Dock = DockStyle.None;
                BtnGiris.Visible = true;
                BtnGiris.Text = "GİRİŞ YAP";
                BtnGiris.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
                BtnGiris.BackColor = Color.FromArgb(22, 160, 133);
                BtnGiris.ForeColor = Color.White;
                BtnGiris.FlatStyle = FlatStyle.Flat;
                BtnGiris.FlatAppearance.BorderSize = 0;
                BtnGiris.FlatAppearance.MouseOverBackColor = Color.FromArgb(26, 188, 156);
                BtnGiris.FlatAppearance.MouseDownBackColor = Color.FromArgb(19, 141, 117);
                BtnGiris.Cursor = Cursors.Hand;
                BtnGiris.Location = new Point(82, 486);
                BtnGiris.Size = new Size(296, 40);

                // Eski mavi "Versiyon" seridini tamamen gizle (mavi serit buydu)
                LblVersiyon.Visible = false;

                // Sade, profesyonel alt bilgi - lacivert zemin uzerinde, seffaf
                var lblFooter = new Label
                {
                    Text = "© 2026  SENKRON ERP  ·  Üretim Yönetim Sistemi",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(120, 140, 175),
                    Font = new Font("Segoe UI", 8.25F, FontStyle.Regular),
                    Size = new Size(440, 18),
                    Location = new Point(10, 537)
                };
                this.Controls.Add(lblFooter);

                // Kapat (X) butonu: eski mavi kutuyu kaldir (lacivert zemine gom)
                var closes = this.Controls.Find("LblClose", true);
                if (closes.Length > 0)
                {
                    var lc = closes[0] as My.Kontrol.Kontroller.MyLabel;
                    if (lc != null)
                    {
                        lc.Appearance.BackColor = Color.FromArgb(15, 30, 64);
                        lc.Appearance.Options.UseBackColor = true;
                        lc.AppearanceHovered.BackColor = Color.FromArgb(192, 57, 43);
                        lc.AppearanceHovered.Options.UseBackColor = true;
                    }
                }

                // Z-sira: kontroller arka planin onunde
                lblHos.BringToFront();
                lblK.BringToFront(); lblS.BringToFront();
                TxtKullanici.BringToFront(); TxtSifre.BringToFront();
                BtnGiris.BringToFront(); lblFooter.BringToFront();
            }
            catch { }
        }
    }
}